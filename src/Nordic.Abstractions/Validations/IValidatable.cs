namespace Nordic.Abstractions.Validations
{
	public interface IValidatable
	{
		object Result { get; }

		bool HasSucceeded { get; }

		// -- methods

		IValidatable Validate();
	}
}
