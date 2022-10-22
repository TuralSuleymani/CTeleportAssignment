namespace CTeleportAssignment.Services.Exceptions
{

	[Serializable]
	public class IataNotFoundException : Exception
	{
		public IataNotFoundException() { } 
		public IataNotFoundException(string message) : base(message) { }
		public IataNotFoundException(string message, Exception inner) : base(message, inner) { }
		protected IataNotFoundException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
