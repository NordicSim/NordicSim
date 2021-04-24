using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nordic.Simulation.AdaptedFriis;
using TeleScope.Logging.Extensions;

namespace Nordic.MSTest.Simulation
{
	[TestClass]
	public class AdaptedFriisTests : TestsBase
	{
		// -- inherits

		[TestInitialize]
		public override void Arrange()
		{
			base.Arrange();
		}

		[TestCleanup]
		public override void Cleanup()
		{
			base.Cleanup();
		}

		// -- Tests

		[TestMethod]
		public void RunAdaptedFriis()
		{
			// arrange
			var sim = new AdaptedFriisSimulator();
			sim.OnExecuting += (o, e) =>
			{
				_log.Trace($"{e.Arguments.Name} started");
			};

			sim.Executed += (o, e) =>
			{
				_log.Trace($"{e.Arguments.Name} finished");
			};

			// act
			
			sim.OnStart().Run();

			var radioArgs = sim.Arguments as AdaptedFriisArgs;

			// assert
			Assert.IsTrue(radioArgs.RxValues.All(f => f != 0), "float should contain a attenuation");

			radioArgs.RxPositions
				.Zip(radioArgs.RxValues, (a, b) => $"Pos: {a} with {b} dBm")
				.ToList()
				.ForEach(s => _log.Trace(s));

		}
	}
}
