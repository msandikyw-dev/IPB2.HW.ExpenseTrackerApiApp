namespace IPB2.HW.ExpenseTrackerApiApp.Models
{
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "Success.";
    }

    public class ResponseDTO<T> : ResponseDTO
    {
        public T? Data { get; set; }
    }
}
