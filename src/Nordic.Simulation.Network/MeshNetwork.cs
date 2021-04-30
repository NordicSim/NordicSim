using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Nordic.Abstractions.Data;
using Nordic.Abstractions.Devices;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;

namespace Nordic.Simulation.Network
{
	public class MeshNetwork : IEnumerable<IDevice>
	{

		// -- fields

		private readonly ILogger<MeshNetwork> _log;
		private readonly List<IDevice> _items;

		// -- properties

		/// <summary>
		/// Gets or sets the communication associations between the devices.
		/// </summary>
		public NetworkMatrix<bool> AssociationMatrix { get; set; }

		/// <summary>
		/// Gets or sets the distances between the devices.
		/// </summary>
		public NetworkMatrix<float> DistanceMatrix { get; set; }

		/// <summary>
		/// Gets or sets the received signal strength (RSS) between the devices.
		/// </summary>
		public NetworkMatrix<float> RssMatrix { get; set; }

		/// <summary>
		/// Gets or sets the angles between the devices.
		/// </summary>
		public NetworkMatrix<Angle> AngleMatrix { get; set; }

		// -- indexter

		public IDevice this[int index] => _items[index];

		// -- constructors

		public MeshNetwork()
		{
			_log = LoggingProvider.CreateLogger<MeshNetwork>();

			_items = new List<IDevice>();

			AssociationMatrix = new NetworkMatrix<bool>();
			DistanceMatrix = new NetworkMatrix<float>();
			RssMatrix = new NetworkMatrix<float>();
			AngleMatrix = new NetworkMatrix<Angle>();
		}

		// -- methods

		public IEnumerator<IDevice> GetEnumerator()
		{
			return _items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(IDevice device)
		{
			_items.Add(device);
		}

		public void AddRange(IDevice[] devices)
		{
			_items.AddRange(devices);
		}




		public void SetupMatrices()
		{
			// execute validation & log
			var results = new MeshNetworkValidator(this)
				.Validate()
				.Result as ValidationResult;

			LogValidationErrors(results);

			int size = _items.Count;

			AssociationMatrix.Init(size);
			DistanceMatrix.Init(size);
			RssMatrix.Init(size);
			AngleMatrix.Init(size, new Angle(float.NaN, float.NaN));
		}

		public float Availability()
		{
			var online = _items.Count(d => d.IsActive);
			var all = _items.Count;
			return (float)online / (float)all;
		}

		private void LogValidationErrors(ValidationResult results)
		{
			if (!results.IsValid)
			{
				foreach (var failure in results.Errors)
				{
					string property = (!string.IsNullOrEmpty(failure.PropertyName) ? $" property: {failure.PropertyName}" : "");
					_log.Error($"Failed validation: {failure.ErrorMessage}{property}");
				}
			}
			else
			{
				_log.Debug($"Validation succeeded.");
			}
		}


	}
}
