using System;
using System.Linq;
using FluentValidation;
using Nordic.Abstractions.Validations;

namespace Nordic.Simulation.MeshNetwork
{
	public class MeshNetworkValidator : AbstractValidator<MeshNetwork>, IValidatable
	{
		// -- fields

		private readonly MeshNetwork _network;

		// -- properties
		
		public object Result { get; private set; }

		public bool HasSucceeded { get; private set; }

		// -- constructor

		public MeshNetworkValidator(MeshNetwork network)
		{
			_network = network;
			SetupRules();
		}

		// -- methods

		public IValidatable Validate()
		{
			var results = base.Validate(_network);

			HasSucceeded = results.IsValid;
			Result = results;

			return this;
		}

		// helper

		private void SetupRules()
		{
			CascadeMode = CascadeMode.Stop;

			RuleFor(net => net)
				.Must(n => ContainsDevices(n))
				.WithMessage("network instance must be present and should contain at least one device");

		}

		private bool ContainsDevices(MeshNetwork network)
		{
			if (network == null) return false;
			if (!network.Any()) return false;
			return true;
		}
	}
}
