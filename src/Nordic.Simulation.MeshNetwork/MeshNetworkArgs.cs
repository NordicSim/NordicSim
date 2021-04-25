using Nordic.Abstractions.Data.Arguments;

namespace Nordic.Simulation.MeshNetwork
{
	public class MeshNetworkArgs : ArgumentsBase
	{
		// --fields

		public const string KEY = "meshnet";
		public const string NAME = "mesh network";

		// -- properties

		public MeshNetwork Network { get; set; }

		/// <summary>
		/// Gets or sets the trigger for the connected simulator to recalculate the network data
		/// </summary>
		public bool NetworkOutdated { get; set; }


		// -- constructor

		public MeshNetworkArgs() : base()
		{
			Key = KEY;
			Name = NAME;

			Reset();
		}

		public override void Reset()
		{
			Network = new MeshNetwork();
			NetworkOutdated = true;
		}
	}
}
