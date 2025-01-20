namespace CTeleportAssignment.Providers.Exceptions
{

	[Serializable]
	public class ProviderException : Exception
	{
		public ProviderException() { }
		public ProviderException(string message) : base(message) { }
		public ProviderException(string message, Exception inner) : base(message, inner) { }
		protected ProviderException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
