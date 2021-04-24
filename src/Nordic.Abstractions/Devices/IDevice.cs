using Nordic.Abstractions.Data;

namespace Nordic.Abstractions.Devices
{
	public interface IDevice
	{
		// -- properties
		string Uuid { get; set; }
		string Name { get; set; }
		string Description { get; set; }
		bool IsActive { get; set; }
		Vertex Position { get; set; }
		Angle Orientation { get; set; }
		PartsRepository Parts { get; }
		ControlBase Controls { get; }
	}
}
