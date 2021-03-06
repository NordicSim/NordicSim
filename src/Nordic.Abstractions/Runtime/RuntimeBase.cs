using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nordic.Abstractions.Data.Arguments;
using Nordic.Abstractions.Events;
using Nordic.Abstractions.Simulation;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;

namespace Nordic.Abstractions.Runtime
{
	/// <summary>
	/// Abstract class with some base functionality to setup and run the desired simulation models
	/// </summary>
	public abstract class RuntimeBase
	{
		// -- fields

		private readonly ILogger<RuntimeBase> _log;

		protected SimulatorRepository _simRepo;

		protected bool _isValid;
		protected bool _isRunning;
		protected bool _stopping;

		// -- properties

		/// <summary>
		/// Gets the specific arguments for the concrete runtime implementation as base class 
		/// </summary>
		public abstract ArgumentsBase Arguments { get; }

		/// <summary>
		/// Gets the information, if the validation was successful or not.
		/// </summary>
		public bool IsValid { get { return _isValid; } }

		/// <summary>
		/// Gets the information, if the simulation is running or not. 
		/// </summary>
		public bool IsRunning { get { return _isRunning; } }

		// -- indexer

		/// <summary>
		/// Gets the repository for the attached simulators.
		/// </summary>
		public SimulatorRepository Simulators => _simRepo;

		// -- events

		/// <summary>
		/// The event gets fired when the simulation starts the first iteration. 
		/// </summary>
		public event SimulatorEventHandler Started;

		/// <summary>
		/// The event gets fired when the simulation stoppes the last iteration. 
		/// </summary>
		public event SimulatorEventHandler Stopped;

		/// <summary>
		/// The event gets fired when the execution of all simulation models has finished one iteration 
		/// </summary>
		public event SimulatorEventHandler IterationPassed;

		// -- constructor

		protected RuntimeBase()
		{
			_log = LoggingProvider.CreateLogger<RuntimeBase>();
			BindSimulators(new SimulatorRepository());
			Started += OnStart;
		}

		// -- methods

		public abstract RuntimeBase With(Action<ArgumentsBase> action);

		/// <summary>
		/// Takes the Simulator Repository instance with all ready-to-go simulators and sets the internal valid-state to invalid.
		/// This ensures that the concretion of this base class implements a validation method and runs it before the simulation. 
		/// </summary>
		/// <param name="simulatorRepo">The repository instance</param>
		/// <returns>Returns the calling instance for method chaining</returns>
		public RuntimeBase BindSimulators(SimulatorRepository simulatorRepo)
		{
			_isValid = false;
			_simRepo = simulatorRepo ?? throw new ArgumentException("The simulation repository must not be null.", nameof(simulatorRepo));
			return this;
		}

		/// <summary>
		/// The conrete runtime implementation implements the validation of all simulation models here.
		/// The Method is called before the iteration of all registered models in the RunAsync method.
		/// </summary>
		/// <returns></returns>
		public abstract bool Validate();

		/// <summary>
		/// Stops the simulation after finishing the current iteration of the simulation.
		/// </summary>
		public void Stop()
		{
			_log.Trace("Simulation runtime stopping");
			_isRunning = false;
			_stopping = true;
		}

		#region RunAsync
		/// <summary>
		/// Start the iteration of the run method of all registered simulation models
		/// without any break condition
		/// </summary>
		/// <returns>The task object representing the async task</returns>
		public async Task RunAsync()
		{
			// go baby go!
			await RunAsync((runtime) => { return true; });
		}

		/// <summary>
		/// Start the iteration of the run method of all registered simulation models
		/// for a defined number of times
		/// </summary>
		/// <param name="count">Determines the number of iterations of all simulation models</param>
		/// <returns>The task object representing the async task</returns>
		public async Task RunAsync(int count)
		{
			int i = 0;
			await RunAsync((runtime) =>
			{
				if (i >= count)
				{
					return false;
				}

				i++;
				return true;
			});
		}

		/// <summary>
		/// Start the iteration of the run method of all registered simulation models
		/// as long as the condition method returns true
		/// </summary>
		/// <param name="condition">A method that determines the condition to continue or to end the simulation</param>
		/// <returns>The task object representing the async task</returns>
		public virtual async Task RunAsync(Func<RuntimeBase, bool> condition)
		{
			_stopping = false;  // reset the stopping flag before entering the async part of the method
			if (!_isValid)
			{
				// stop running
				_log.Trace("Simulation is invalid or was not validated");
				Stop();
				return;
			}

			await Task.Run(() =>
			{
				_log.Info($"# Start of simulation");
				_isRunning = condition(this);

				// fire event when iteration starts
				Started?.Invoke(this, new SimulatorEventArgs(Arguments));

				var list = _simRepo.SortActiveSimulators();
				while (_isRunning)
				{
					foreach (ISimulatable sim in list)
					{
						sim.Run();
					}

					// fire event that one iteration of all simulation models has finished
					IterationPassed?.Invoke(this, new SimulatorEventArgs(Arguments));

					// separate flag to ensure that the condition method does not overwrite the stop action
					if (!_stopping)
					{
						_isRunning = condition(this);
					}
				}
			})
			.ContinueWith((t) =>
			{
				if (t.Status == TaskStatus.Faulted)
				{
					_log.Error($"{t.Exception.InnerExceptions.Count} exception occured during simulation.");
					foreach (var e in t.Exception.InnerExceptions)
					{
						_log.Critical(e);
					}
				}

				_log.Info($"# End of simulation");
				Stopped?.Invoke(this, new SimulatorEventArgs(Arguments));
			});
		}
		#endregion

		private void OnStart(object sender, SimulatorEventArgs e)
		{
			_simRepo.SortActiveSimulators()
				.ToList()
				.ForEach(s => s.OnStart());
		}

		/// <summary>
		/// Returns the Name property of the Arguments instance
		/// </summary>
		/// <returns>result string</returns>
		public override string ToString() => Arguments.Name;

	}
}
