using System;
using Nordic.Abstractions.Constants;
using Nordic.Abstractions.Data;
using Nordic.Abstractions.Devices;

namespace Nordic.Simulation.MeshNetwork.Devices
{
	public class SimpleDevice : IDevice
	{
		// -- properties

		public string Uuid { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsActive { get; set; }
		public Vertex Position { get; set; }
		public Angle Orientation { get; set; }
		public PartsRepository Parts { get; }
		public ControlBase Controls { get; }

		// -- constructor

		public SimpleDevice()
		{
			Uuid = Guid.NewGuid().ToString();
			Name = Const.Device.Name;
			Description = Const.Device.Description;
			Position = new Vertex(Const.Device.PosX, Const.Device.PosY, Const.Device.PosZ);
			Orientation = new Angle(Const.Antenna.Azimuth, Const.Antenna.Elevation);
			IsActive = true;

			Parts = new PartsRepository();
			Controls = new SimpleControls(this);
		}

		// -- public methods

		public override string ToString() => $"{Name} {Position}";
	}
}
