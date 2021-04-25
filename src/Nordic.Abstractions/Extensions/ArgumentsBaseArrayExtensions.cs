using System.Linq;
using Nordic.Abstractions.Data.Arguments;

namespace Nordic.Abstractions.Extensions
{
	public static class ArgumentsBaseArrayExtensions
	{
		// -- Name

		public static bool ContainsName(this ArgumentsBase[] args, string name)
		{
			return args.Any(a => a.Name == name);
		}

		public static ArgumentsBase GetByName(this ArgumentsBase[] args, string name)
		{
			return args.FirstOrDefault(a => a.Name == name);
		}

		public static void SetByName(this ArgumentsBase[] args, string name, ArgumentsBase value)
		{
			var index = args.GetIndexByName(name);
			args[index] = value;
		}

		public static int GetIndexByName(this ArgumentsBase[] args, string name)
		{
			return args.ToList().IndexOf(args.GetByName(name));
		}
	}
}
