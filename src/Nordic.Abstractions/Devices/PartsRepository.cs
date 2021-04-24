using System.Collections.Generic;
using System.Linq;
using Nordic.Abstractions.Enumerations;

namespace Nordic.Abstractions.Devices
{
    public class PartsRepository
    {
        // -- fields

        private readonly List<PartBase> _items;

        // -- properties

        public string Name { get; set; }

        public bool HasPowerSupply => _items.Any(p => p.Type == PartTypes.PowerSupply);

        // -- constructors

        public PartsRepository()
        {
            Name = "part repository";
            _items = new List<PartBase>();
        }

        // -- methods

        public PartBase GetPowerSupply()
        {
            return _items.FirstOrDefault(p => p.Type == PartTypes.PowerSupply);
        }

        //public void Add(BatteryPack battery)
        //{
        //    _items.Add(battery);

        //}
    }
}