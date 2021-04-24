using System.Collections.Generic;
using Nordic.Abstractions.Constants;
using Nordic.Abstractions.Data;
using Nordic.Abstractions.Data.Arguments;

namespace Nordic.Simulation.AdaptedFriis
{
	public class AdaptedFriisArgs : ArgumentsBase
	{
		// -- fields

		public const string KEY = "friis";
		public const string NAME = "adapted friis";

		// -- properties

		public float TxFrequencyMHz { get; set; }
		public float TxWavelength { get { return Const.Channel.Radio.FreqToMeter(TxFrequencyMHz); } }
		public float TxPowerDBm { get; set; }

		public float AttenuationExponent { get; set; }
		public float AttenuationOffset { get; set; }
		public bool UseObstacles { get; set; }
		public RadioCuboid RadioBox { get; set; }
		public Vertex[] RxPositions { get; set; }
		public float[] RxValues { get; set; }
		public List<float[]> RxColors { get; set; }

		// -- constructors

		public AdaptedFriisArgs() : base()
		{
			Key = KEY;
			Name = NAME;

			Reset();
		}

		// -- methods

		public override void Reset()
		{
			// default settings
			RadioBox = new RadioCuboid();
			AttenuationExponent = Const.Channel.Radio.AttenuationExponent;
			AttenuationOffset = Const.Channel.Radio.AttenuationOffset;
			TxFrequencyMHz = Const.Channel.Radio.FrequencyMhz;
			TxPowerDBm = Const.Channel.Radio.PowerDbm;
		}

		public virtual void UpdatePositions()
		{
			RxPositions = RadioBox.CreateRxPositions();
		}
	}
}
