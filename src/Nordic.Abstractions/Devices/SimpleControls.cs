using System;
using System.Collections.Generic;
using System.Text;

namespace Nordic.Abstractions.Devices
{
	public class SimpleControls : ControlBase
	{
		public SimpleControls(IDevice device) : base(device)
		{
		}

		public override void On()
		{
			base._device.IsActive = true;
		}

		public override void Off()
		{
			base._device.IsActive = false;
		}
	}
}
