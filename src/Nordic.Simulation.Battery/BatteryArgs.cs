using System;
using System.Collections.Generic;
using Nordic.Abstractions.Constants;
using Nordic.Abstractions.Data.Arguments;

namespace Nordic.Simulation.Battery
{
	public class BatteryArgs : ArgumentsBase
	{
		// -- fields

		private readonly List<BatteryPack> _batteries;
		private readonly Dictionary<Guid, float> _dischargeCurrents;

		// -- properties

		public IEnumerable<BatteryPack> Batteries => _batteries;

		public TimeSpan CycleDuration { get; set; }

		// -- constructor

		public BatteryArgs() : base()
		{
			Key = "";
			Name = "";
			_batteries = new List<BatteryPack>();
			_dischargeCurrents = new Dictionary<Guid, float>();
			Reset();
		}

		// -- methods

		public BatteryPack AddBattery()
		{
			return AddBattery(new BatteryPack());
		}

		public BatteryPack AddBattery(BatteryPack battery)
		{
			_batteries.Add(battery);
			_dischargeCurrents.Add(battery.Uid, 0);

			return battery;
		}

		public override void Reset()
		{
			_batteries.Clear();
			_dischargeCurrents.Clear();
			CycleDuration = Const.Energy.DefaultCycleDuration;
		}

		internal float GetDischargeCurrent(Guid uid)
		{
			return _dischargeCurrents[uid];
		}

		public void UpdateDischargeCurrent(Guid uid, float current)
		{
			_dischargeCurrents[uid] = current;
		}
	}
}
