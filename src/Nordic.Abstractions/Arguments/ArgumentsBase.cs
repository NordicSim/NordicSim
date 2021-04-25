using Nordic.Abstractions.Enumerations;

namespace Nordic.Abstractions.Data.Arguments
{
	/// <summary>
	/// This class is an abstract base class for all concrete argument implementations used by the simulation models
	/// </summary>
	public abstract class ArgumentsBase
	{
		// -- properties

		/// <summary>
		/// Gets or sets the index of the argument. This defines the order of the simulation steps.
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Key { get; protected set; }

		/// <summary>
		/// Gets or sets the name of the specific arguments class
		/// </summary>
		public string Name { get; protected set; }

		/// <summary>
		/// Gets or sets if the corresponding simulator shall be used in the runtime or not.
		/// </summary>
		public bool IsActive { get; private set; }

		// -- constructor

		protected ArgumentsBase()
		{
			Index = -1;
			Activate();
		}

		// -- methods

		/// <summary>
		/// Gets a hard coded representation of the arguments
		/// </summary>
		/// <returns>The default instance of the arguments implementation</returns>
		public abstract void Reset();

		/// <summary>
		/// 
		/// </summary>
		public void Activate()
		{
			IsActive = true;

		}

		/// <summary>
		/// 
		/// </summary>
		public void Deactivate()
		{
			IsActive = false;
		}

		/// <summary>
		/// Returns the Name property of the abstract base class
		/// </summary>
		/// <returns>result string</returns>
		public override string ToString() => $"args: {Name}";
	}
}
