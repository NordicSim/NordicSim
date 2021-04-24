namespace Nordic.Abstractions.Devices
{
	public abstract class ControlBase
	{
		protected IDevice _device;

		protected ControlBase(IDevice device)
		{
			_device = device;
		}

		public abstract void Off();
		public abstract void On();
	}
}
