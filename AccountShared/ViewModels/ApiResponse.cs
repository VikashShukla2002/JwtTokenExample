namespace AccountShared.ViewModels
{
    public class ApiResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }

        public int ErrorCode { get; set; }
    }
}
