namespace CTeleportAssignment.Services.Exceptions
{

	[Serializable]
	public class InvalidIataProvidedException : Exception
	{
		public InvalidIataProvidedException() { }
		public InvalidIataProvidedException(string message) : base(message) { }
		public InvalidIataProvidedException(string message, Exception inner) : base(message, inner) { }
		protected InvalidIataProvidedException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
