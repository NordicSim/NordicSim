using System;
using Nordic.Abstractions.Constants;
using Nordic.Abstractions.Data.Arguments;

namespace Nordic.Runtime
{
	public class RuntimeArgs : ArgumentsBase
	{
		// -- fields

		public const string KEY = "run";
		public const string NAME = "runtime";

		// -- properties

		/// <summary>
		/// Gets or sets the local DateTime, when the simulation was started.
		/// </summary>
		public DateTime StartTime { get; set; }


		/// <summary>
		/// Gets or sets the theoretical realtime that has passed.
		/// </summary>
		public TimeSpan SimulatedTime { get; set; }

		/// <summary>
		/// Gets or sets the time the simulation was running.
		/// </summary>
		public TimeSpan ElapsedTime { get; set; }

		/// <summary>
		/// Gets or sets the number of iterations the runtime has completed. 
		/// </summary>
		public ulong Iterations { get; set; }

		/// <summary>
		/// Gets or sets the duration of one iteration of the runtime.
		/// </summary>
		public TimeSpan CycleDuration { get; set; }

		// -- constructor

		public RuntimeArgs() : base()
		{
			Key = KEY;
			Name = NAME;

			CycleDuration = TimeSpan.FromSeconds(Const.Runtime.IncrementSeconds);
		}

		// -- methods

		public override void Reset()
		{
			ResetTime();
		}

		public void ResetTime()
		{
			StartTime = DateTime.Now;
			SimulatedTime = new TimeSpan();
			Iterations = 0;
		}
	}
}
