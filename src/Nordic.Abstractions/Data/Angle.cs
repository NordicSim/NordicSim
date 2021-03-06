using System;
using Nordic.Abstractions.Constants;

namespace Nordic.Abstractions.Data
{
	[Serializable]
	public class Angle
	{
		public Angle()
		{
			Azimuth = Const.Antenna.Azimuth;
			Elevation = Const.Antenna.Elevation;
		}

		public Angle(float azimuth, float elevation)
		{
			this.Azimuth = azimuth;
			this.Elevation = elevation;
		}

		public override string ToString()
		{
			return $"Az: {Azimuth}, El: {Elevation}";
		}

		/// <summary>
		/// The east-west angle on a sphere
		/// In antenna characteristics it is the horizontal angle
		/// </summary>
		public float Azimuth { get; set; }

		/// <summary>
		/// The north-south angle on a sphere
		/// In antenna characteristics it is the vertical angle
		/// </summary>
		public float Elevation { get; set; }
	}
}
