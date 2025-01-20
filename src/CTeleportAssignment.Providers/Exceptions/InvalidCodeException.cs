namespace CTeleportAssignment.Providers.Exceptions
{

    [Serializable]
    public class InvalidCodeException : ApplicationException
    {
        public int StatusCode { get; }
        public InvalidCodeException(string message) : base($"Invalid IATA code provided! {message}")
        {
            StatusCode = 400;
        }
        public InvalidCodeException(string message, Exception inner) : base(message, inner) { }
        protected InvalidCodeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
}
