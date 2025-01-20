namespace CTeleportAssignment.WebAPI.Models
{
    public class ApiResponse<T> where T : class
    {
        public ApiResponse(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
        public ApiErrorDetail ErrorDetail { get; set; }

    }
}
