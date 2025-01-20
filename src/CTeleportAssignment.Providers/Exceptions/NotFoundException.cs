
namespace CTeleportAssignment.Providers.Exceptions
{

	[Serializable]
	public class NotFoundException : Exception
	{
        public int StatusCode { get; }
        public NotFoundException(string message) : base($"Given IATA Code doesn't exist! {message}")
        {
            StatusCode = 404;
        }
        public NotFoundException(string message, Exception inner) : base(message, inner) { }
		protected NotFoundException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
