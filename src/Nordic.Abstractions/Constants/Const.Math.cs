namespace Nordic.Abstractions.Constants
{
	/// <summary>
	/// The partial class contains constant fields and methods that should not be applied as extension methods or
	/// within outer layers inside the simulation models.
	/// The other partial classes have a separate scope in line with the simulation models
	/// </summary>
	public static partial class Const
	{
		/// <summary>
		/// Container class for basic an simple mathematical constant functions
		/// </summary>
		public static class Math
		{
			/// <summary>
			/// Calculates the Degree from a given float the represents an angle as Radian
			/// </summary>
			/// <param name="rad">the input angle as Radian value</param>
			/// <returns>The Degree value</returns>
			public static float ToDegree(float rad) => ((rad * 180) / (float)global::System.Math.PI);

			/// <summary>
			/// Calculates the Radian from a given float the represents an angle as Degree
			/// </summary>
			/// <param name="deg">the input angle as Degree value</param>
			/// <returns>The Radian value</returns>
			public static float ToRadian(float deg) => ((deg * (float)global::System.Math.PI) / 180);

			/// <summary>
			/// Gets the fractional part of a floating point number.
			/// </summary>
			/// <param name="number"></param>
			/// <returns></returns>
			public static float Fract(object input)
			{
				string[] inputStr = input.ToString().Split(new char[] { ',', '.' });

				if (inputStr.Length == 2)
				{
					string fract = (inputStr[0].Contains("-") ? "-0," : "0,") + inputStr[1];
					return float.Parse(fract);
				}
				else
				{
					return 0;
				}
			}
		}
	}
}
