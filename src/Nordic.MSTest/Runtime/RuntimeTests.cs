using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nordic.Simulation.AdaptedFriis;
using Nordic.Simulation.MeshNetwork;
using Nordic.Runtime;
using Nordic.Abstractions.Simulation;
using TeleScope.Logging.Extensions;

namespace Nordic.MSTest.Runtime
{
	[TestClass]
	public class RuntimeTests : TestsBase
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
		public async Task SimulateRuntimeAsync()
		{
			// arrange

			// radio channel
			var radioSim = new AdaptedFriisSimulator();

			// antenna
			//var antennaArgs = simArgs.GetByName(Models.Antenna.Spheric.Name) as SphericAntennaArgs;
			//antennaArgs.Index = 20;
			//base.LoadAntennaData(antennaArgs);

			// network
			var networkSim = new MeshNetworkSimulator();
			BuildMeshNetwork(networkSim.Arguments as MeshNetworkArgs);

			// pack all simulators in a reop
			var simRepo = new SimulatorRepository();
			simRepo.AddRange(new ISimulatable[] {
				radioSim,
				networkSim
			});

			// final setup, cross-bind some arguments
			var runner = new Runner();

			runner.BindSimulators(simRepo);


			runner.Started += (o, e) =>
			{
				_log.Trace($"Runner started.");
			};

			runner.IterationPassed += (o, e) =>
			{
				_log.Trace($"Runner passed iteration");
			};

			runner.Stopped += (o, e) =>
			{
				_log.Trace($"Runner stopped.");
			};

			// act
			int iterations = 5;
			if (!runner.Validate())
			{
				Assert.Fail("Error on validating the simulation runtime.");
			}

			_log.Trace($"RunAsync for {iterations} times");
			await runner.RunAsync(iterations);

			// assert
			var runArgs = runner.Arguments as RuntimeArgs;
			Assert.IsTrue(runArgs.Iterations == 5, $"simulation should have passed {iterations} iterations");
		}
	}
}
