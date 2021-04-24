using System;
using System.Runtime.Serialization;
using Nordic.Abstractions.Simulation;

namespace Nordic.Abstractions.Exceptions
{
	[Serializable]
	public class SimulatorException : Exception
	{
		public SimulatorException()
		{
		}

		public SimulatorException(string message) : base(message)
		{
		}

		public SimulatorException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected SimulatorException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public SimulatorException(ISimulatable simulator, string message) : base($"{simulator.Name} failed: {message}")
		{
		}
	}
}
