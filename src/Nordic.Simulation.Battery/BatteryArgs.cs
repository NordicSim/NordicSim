using System.Collections.Generic;
using Nordic.Abstractions.Data.Arguments;

namespace Nordic.Simulation.Battery
{
	public class BatteryArgs : ArgumentsBase
	{
		// -- fields

		private readonly List<BatteryPack> _batteries;
		// -- properties

		public IEnumerable<BatteryPack> Batteries => _batteries;

		public BatteryArgs() : base()
		{
			Key = "";
			Name = "";
			_batteries = new List<BatteryPack>();

			Reset();
		}

		// -- methods

		public void AddBattery()
		{
			_batteries.Add(new BatteryPack());
		}

		public void AddBattery(BatteryPack battery)
		{
			_batteries.Add(battery);
		}

		public override void Reset()
		{

		}
	}
}
