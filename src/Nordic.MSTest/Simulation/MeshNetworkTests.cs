using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nordic.Abstractions.Data;
using Nordic.Simulation.MeshNetwork;
using Nordic.Simulation.MeshNetwork.Devices;
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
				var netArgs = args as MeshNetworkArgs;
				var rand = new Random();
				var randomDevices = Enumerable.Range(0, 10)
					.Select(i => new SimpleDevice
					{
						Name = $"dev_{i}",
						Position = new Vertex(rand.Next(0, 100), rand.Next(0, 100), rand.Next(0, 100))
					})
					.ToArray();

				netArgs.Network.AddRange(randomDevices);
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
