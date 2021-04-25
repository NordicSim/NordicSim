using System;
using Nordic.Abstractions.Data.Arguments;
using Nordic.Abstractions.Enumerations;
using Nordic.Abstractions.Events;

namespace Nordic.Abstractions.Simulation
{
	public interface ISimulatable
	{

		// -- properties

		/// <summary>
		///	
		/// </summary>
		ArgumentsBase Arguments { get; }

		/// <summary>
		/// 
		/// </summary>
		SimulationTypes Type { get; }

		// -- events

		/// <summary>
		/// Shall be fired at first, when the execution of the simulation model starts 
		/// </summary>
		event SimulatorEventHandler OnExecuting;

		/// <summary>
		/// Shall be fired at last, when the execution of the simulation model has finished 
		/// </summary>
		event SimulatorEventHandler Executed;

		// -- methods

		/// <summary>
		/// Attaches the specific arguments to the simulator concretion 
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		ISimulatable With(Action<ArgumentsBase> action);

		/// <summary>
		/// Gets executed when the simulation starts the first time.
		/// </summary>
		ISimulatable OnStart();

		/// <summary>
		/// Runs the implementation of the simulation model
		/// </summary>
		void Run();
		
	}
}
