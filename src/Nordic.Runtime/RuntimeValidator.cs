using Nordic.Abstractions.Simulation;
using Nordic.Abstractions.Validations;

namespace Nordic.Runtime
{
	public class RuntimeValidator : IValidatable
	{
		// -- fields

		private readonly SimulatorRepository _simulators;

		// -- properties

		public object Result { get; private set; }

		public bool HasSucceeded { get; private set; }

		// -- constructor

		public RuntimeValidator(SimulatorRepository simulatorRepository)
		{
			_simulators = simulatorRepository;
		}

		// -- methods

		public IValidatable Validate()
		{
			if (_simulators.Count > 0)
			{
				HasSucceeded = true;
			}

			return this;
		}
	}
}
