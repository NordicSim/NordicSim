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
		int Index { get; }

		/// <summary>
		/// Gets the name property of the simulation model
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets the Identifier of the simulation model
		/// </summary>
		string Key { get; }

		/// <summary>
		/// Gets the type of the simulation model
		/// </summary>
		SimulationTypes Type { get; }

		/// <summary>
		/// 
		/// </summary>
		bool IsActive { get; }

		/// <summary>
		///	
		/// </summary>
		ArgumentsBase Arguments { get; }

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
