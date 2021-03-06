using Nordic.Abstractions.Data.Arguments;
using Nordic.Abstractions.Runtime;
using Nordic.Abstractions.Simulation;

namespace Nordic.Abstractions
{
    public abstract class FactoryBase
    {
        // -- fields

        // -- properties

        //public virtual ArgumentsBase[] SimulationArguments => Simulators.AllArguments();

        protected SimulatorRepository Simulators { get; set; }

        // -- constructors

        protected FactoryBase()
        {

        }

        // -- methods

        public abstract ArgumentsBase NewArgument(string name);

        public abstract ISimulatable NewSimulator(string name, RuntimeBase runtime);

        public abstract ArgumentsBase[] GetPredefinedArguments();

        public abstract ISimulatable RegisterSimulator(ISimulatable simulator, ArgumentsBase args);

        public abstract RuntimeBase SetupRuntime(ArgumentsBase[] args, RuntimeBase runtime);

    }
}