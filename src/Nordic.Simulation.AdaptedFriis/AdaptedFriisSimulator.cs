using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Nordic.Abstractions.Data;
using Nordic.Abstractions.Data.Arguments;
using Nordic.Abstractions.Enumerations;
using Nordic.Abstractions.Events;
using Nordic.Abstractions.Simulation;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;

namespace Nordic.Simulation.AdaptedFriis
{
	[Serializable]
	public class AdaptedFriisSimulator : ISimulatable
	{
		// -- fields

		private readonly ILogger<AdaptedFriisSimulator> _log;
		private readonly AdaptedFriisArgs _args;

		// -- events

		public event SimulatorEventHandler OnExecuting;
		public event SimulatorEventHandler Executed;

		// -- properties

		public int Index => _args.Index;

		public string Key => _args.Key;
		public string Name => _args.Name;

		public SimulationTypes Type => SimulationTypes.Channel;

		public bool IsActive => _args.Active;

		public ArgumentsBase Arguments => _args;

		// -- constructors

		public AdaptedFriisSimulator()
		{
			_log = LoggingProvider.CreateLogger<AdaptedFriisSimulator>();
			_args = new AdaptedFriisArgs();
		}

		// -- methods

		public ISimulatable With(Action<ArgumentsBase> action)
		{
			action(_args);
			return this;
		}

		public ISimulatable OnStart()
		{
			_args.UpdatePositions();
			return this;
		}

		public void Run()
		{
			OnExecuting?.Invoke(this, new SimulatorEventArgs(_args));


			var tx = new Vertex();
			float a = _args.AttenuationExponent;
			float c = GetFriisConstant(_args.TxWavelength);
			_args.RxValues = new float[_args.RadioBox.TotalData];

			/*
             * - parallelize the loop
             * - make a wrapper object to get the Index and the rx position vector 'Rx'
             * - calculate the adapted friis transmission and save it in the corresponding RxValues[d.Index]
             */
			_args.RxPositions
				.AsParallel()
				.Select((rx, i) => new { Index = i, Rx = rx })
				.ForAll(d => _args.RxValues[d.Index] = GetAdaptedFriisAttenuation(c, Vertex.GetLength(tx, d.Rx), a));

			// log finished process
			Executed?.Invoke(this, new SimulatorEventArgs(_args));
			_log.Trace($"{Name} calculated {_args.RadioBox.TotalData} values.");
		}

		/// <summary>
		/// Adapted friis transmission
		/// </summary>
		/// <param name="c">friis constant</param>
		/// <param name="r">radius or distance between transmitter position (tx) and receiver position (rx)</param>
		/// <param name="a">attenuation exponent</param>
		/// <returns></returns>
		private float GetAdaptedFriisAttenuation(float c, float r, float a)
		{
			// adapted friis transmission = c + 20 * log( r ^ a)
			var result = c + 20 * (float)(Math.Log10(Math.Pow(r, a)));
			return result;
		}

		/// <summary>
		/// precalculates the constant part of the adapted friis transmission
		/// to have this calculation only once in a simulation run 
		/// </summary>
		/// <param name="wavelength"></param>
		/// <returns></returns>
		public float GetFriisConstant(float wavelength)
		{
			return 20 * (float)Math.Log10((4 * Math.PI / wavelength));
		}

	
	}
}
