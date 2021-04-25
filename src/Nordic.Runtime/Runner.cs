using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nordic.Abstractions.Data.Arguments;
using Nordic.Abstractions.Events;
using Nordic.Abstractions.Runtime;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;

namespace Nordic.Runtime
{
	/// <summary>
	/// Implements the abstract class RuntimeBase and adds some behavior for validation and conrete runtime arguments
	/// </summary>
	public class Runner : RuntimeBase
	{
		// -- fields

		private readonly ILogger<Runner> _log;

		private readonly RuntimeArgs _args;

		private Stopwatch _watch;

		// -- properties

		/// <summary>
		/// Gets the concrete arguments of type RuntimeArgs  
		/// </summary>
		public override ArgumentsBase Arguments => _args;


		// -- constructor

		/// <summary>
		/// The constructor gets the concrete validator injected and instanciates the concrete arguments of RuntimeArgs type
		/// </summary>
		/// <param name="validator">The validator concretion could be of type BasicValidator or a derived class</param>
		public Runner() : base()
		{
			_log = LoggingProvider.CreateLogger<Runner>();

			_args = new RuntimeArgs();

			base.Started += OnStarted;
			base.Stopped += OnStopped;
			base.IterationPassed += OnIterationPassed;

		}

		// -- methods

		/// <summary>
		/// The arguments must be of type RuntimeArgs.
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public override RuntimeBase With(Action<ArgumentsBase> action)
		{
			action(_args);
			return this;
		}

		/// <summary>
		/// Validate the simulation models and their arguments
		/// </summary>
		/// <returns>Returns 'true' if there are no errors otherwise 'false'.</returns>
		public override bool Validate()
		{
			// execute validation
			var validator = new RuntimeValidator(_simRepo).Validate();
			_isValid = validator.HasSucceeded;

			if (!validator.HasSucceeded)
			{
				_log.Error($"Validation failed.");
			}
			else
			{
				_log.Info("Validation passed.");
			}

			return validator.HasSucceeded;
		}

		/// <summary>
		/// Start the iteration of the run method of all registered simulation models
		/// as long as the condition method returns true
		/// The overide adds the start time to the concrete RuntimeArgs instance.
		/// </summary>
		/// <param name="condition">A method that determines the condition to continue or to end the simulation</param>
		/// <returns>The task object representing the async task</returns>
		public override Task RunAsync(Func<RuntimeBase, bool> condition)
		{
			_args.ResetTime();
			return base.RunAsync(condition);
		}

		// -- event methods

		private void OnStarted(object sender, SimulatorEventArgs e)
		{
			_watch = new Stopwatch();
			_watch.Start();
		}
		
		private void OnStopped(object sender, SimulatorEventArgs e)
		{
			_watch.Stop();
			_args.ElapsedTime = _watch.Elapsed;
			_log.Trace($"Duration of simulation: {_args.ElapsedTime}.");
		}

		private void OnIterationPassed(object sender, SimulatorEventArgs e)
		{
			_args.Iterations++;
			_args.SimulatedTime = _args.SimulatedTime.Add(_args.CycleDuration);
			_log.Trace($"{_args.Iterations} iterations at simulated duration: {_args.SimulatedTime}");
		}
	}
}
