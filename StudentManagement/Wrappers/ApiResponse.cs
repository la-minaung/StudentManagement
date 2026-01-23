namespace StudentManagement.Wrappers
{
    public class ApiResponse<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;
        public string[]? Errors { get; set; }
        public T Data { get; set; }

        public ApiResponse() { }

        // Constructor for Success
        public ApiResponse(T data, string message = null)
        {
            Succeeded = true;
            Message = message ?? "Request successful.";
            Data = data;
            Errors = null;
        }

        // Constructor for Failure
        public ApiResponse(string message, string[]? errors = null)
        {
            Succeeded = false;
            Message = message;
            Errors = errors;
            Data = default;
        }
    }
}
