using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nordic.Abstractions.Data;
using Nordic.Simulation.Network;
using Nordic.Simulation.Network.Devices;
using TeleScope.Logging.Extensions;

namespace Nordic.MSTest.Simulation
{
	[TestClass]
	public class MeshNetworkTests : TestsBase
	{
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

		// -- tests

		[TestMethod]
		public void RunNetwork()
		{
			// arrange
			var netSim = new MeshNetworkSimulator();

			netSim.With((args) =>
			{
				base.BuildMeshNetwork(args as MeshNetworkArgs);
			});
			netSim.OnExecuting += (o, e) =>
			{
				_log.Trace($"{e.Arguments.Name} started");
			};

			netSim.Executed += (o, e) =>
			{
				_log.Trace($"{e.Arguments.Name} finished");
			};

			// act
			netSim.OnStart()
				.Run();

			// assert
			var netArgs = netSim.Arguments as MeshNetworkArgs;

			netArgs.Network.DistanceMatrix.Each((r, c, v) =>
			{
				Assert.IsTrue(v > 0, $"position at row '{r}' and col '{c}' should not be '{v}'");
				_log.Trace($"{r}:{c} -> distance: {v}");
				return v;
			});

			netArgs.Network.AngleMatrix.Each((r, c, v) =>
			{
				Assert.IsTrue(!float.IsNaN(v.Azimuth), $"Azimuth at position at row '{r}' and col '{c}' should not be NaN");
				Assert.IsTrue(!float.IsNaN(v.Elevation), $"Elevation at position at row '{r}' and col '{c}' should not be NaN");
				_log.Trace($"{r}:{c} -> angle: {v}");
				return v;
			});
		}
	}
}
