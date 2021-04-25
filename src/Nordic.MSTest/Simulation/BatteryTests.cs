using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nordic.Simulation.Battery;
using TeleScope.Logging.Extensions;

namespace Nordic.MSTest.Simulation
{
	[TestClass]
	public class BatteryTests : TestsBase
	{
		private BatteryPack _battery;

		[TestInitialize]
		public override void Arrange()
		{
			base.Arrange();

			_battery = new BatteryPack();
		}

		[TestCleanup]
		public override void Cleanup()
		{
			base.Cleanup();
		}

		// -- tests

		[TestMethod]
		public void SetupBatteryPack()
		{
			// arrange
			int sec = 3;

			// act
			_battery.State.Now.AddTime(new TimeSpan(0, 0, sec));

			// assert
			Assert.IsTrue(_battery.State.Initial.Charge != 0 && _battery.State.Now.Charge == 0, "charge of the battery is not valid");
			Assert.IsTrue(_battery.State.Initial.ElapsedTime.TotalSeconds == 0, $"battery should be initialized with no elapsed time");
			Assert.IsTrue(_battery.State.Now.ElapsedTime.TotalSeconds >= sec, $"battery should be used at leased {sec} seconds");
		}

		[TestMethod]
		public void RunBatterySimulation()
		{
			// arrange
			var batterySim = new BatteryPackSimulator();
			batterySim.With((args) =>
			{
				var batteryArgs = args as BatteryArgs;
				_battery = batteryArgs.AddBattery(_battery);
				batteryArgs.UpdateDischargeCurrent(_battery.Uid, 150);
			});

			var i = 0;
			var limit = 10000;
			var watch = new Stopwatch();

			// act
			watch.Start();
			while (!_battery.State.IsDepleted && watch.ElapsedMilliseconds < limit)
			{
				batterySim.Run();
				i++;
			}
			watch.Stop();
			_log.Trace($"Battery simulation ends after {i} iterations and {watch.ElapsedMilliseconds}ms with depleted battery: {_battery.State.IsDepleted}.");

			// assert
			var s = _battery.State;

			Assert.AreNotEqual(s.Initial.ElapsedTime, s.Now.ElapsedTime, "ElapsedTime should not be equal");
			Assert.AreNotEqual(s.Initial.Charge, s.Now.Charge, "Charge should not be equal");
			Assert.AreNotEqual(s.Initial.SoD, s.Now.SoD, "SoD should not be equal");
			Assert.AreNotEqual(s.Initial.Voltage, s.Now.Voltage, "Voltage should not be equal");
			Assert.IsTrue(s.IsDepleted, "battery should be empty");
		}
	}
}
