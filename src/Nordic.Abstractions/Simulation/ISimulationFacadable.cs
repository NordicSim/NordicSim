using Nordic.Abstractions.Data.Arguments;
using Nordic.Abstractions.Runtime;

namespace Nordic.Abstractions.Simulation
{
	public interface ISimulationFacadable
	{
		void RegisterPredefined(RuntimeBase runtime);

		ISimulatable Register(ISimulatable simulator);

		ISimulatable Register(ISimulatable simulator, ArgumentsBase args);

		ISimulatable Register(ISimulatable simulator, ArgumentsBase[] argsArray);

		// -- properties

		SimulatorRepository Simulators { get; }

	}
}
