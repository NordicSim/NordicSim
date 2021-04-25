using System.Collections.Generic;
using System.Linq;
using Nordic.Abstractions.Enumerations;

namespace Nordic.Abstractions.Simulation
{
	public class SimulatorRepository
	{
		// -- fields

		private List<ISimulatable> _items;

		// -- indexer

		/// <summary>
		/// Key-based intexer to be able to get the instance with the same Key.
		/// </summary>
		/// <param name="key">The key property in the simulator instances</param>
		/// <returns>the first instance of T with the matching Id property</returns>
		public ISimulatable this[string key] => _items.FirstOrDefault(s => s.Arguments.Key == key);

		public ISimulatable this[SimulationTypes type] => _items.FirstOrDefault(s => s.Type == type);

		// -- properties

		public int Count => _items.Count;


		// -- constructor(s)

		public SimulatorRepository() : base()
		{
			_items = new List<ISimulatable>();
		}

		// -- methods

		public void Add(ISimulatable simulator)
		{
			_items.Add(simulator);
		}

		public bool Contains(ISimulatable simulator)
		{
			return _items.Contains(simulator);
		}

		public void Clear()
		{
			if (_items == null)
			{
				_items = new List<ISimulatable>();
			}
			else
			{
				_items.Clear();
			}
		}

		public void AddRange(IEnumerable<ISimulatable> simulatables)
		{
			_items.AddRange(simulatables);

		}

		public IEnumerable<ISimulatable> SortActiveSimulators()
		{
			return _items
				.Where(s => s.Arguments.IsActive)
				.OrderBy(s => s.Arguments.Index);
		}

		public ISimulatable GetByName(string name)
		{
			return _items.FirstOrDefault(s => s.Arguments.Name == name);
		}
	}
}
