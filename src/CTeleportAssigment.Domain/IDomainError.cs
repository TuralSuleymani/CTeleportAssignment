namespace CTeleportAssigment.Domain
{
    public interface IDomainError
    {
        string? ErrorMessage { get; init; }
        ErrorType ErrorType { get; init; }
        public List<string>? Errors { get; init; }
    }
}
