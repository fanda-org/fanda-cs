namespace Fanda.Dto.Base
{
    public class ErrorDto
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}