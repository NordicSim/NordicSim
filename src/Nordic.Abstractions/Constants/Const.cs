namespace Nordic.Abstractions.Constants
{
	/// <summary>
	/// The partial class contains constant fields and methods that should not be applied as extension methods or
	/// within outer layers inside the simulation models.
	/// The other partial classes have a separate scope in line with the simulation models
	/// </summary>
	public static partial class Const
	{
		// add basic constants or other stuff here...

		public static class Runtime
		{
			public const double IncrementSeconds = 10;
		}

	}
}
