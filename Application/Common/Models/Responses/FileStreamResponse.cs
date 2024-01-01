using System.IO;

namespace Application.Common.Models.Responses
{
    public class FileStreamResponse : DefaultResponse
    {
        public FileStream? FileStream { get; set; }

        public string? FileFormat { get; set; }

        public string? FileName { get; set; }

        public FileStreamResponse() { }

        public FileStreamResponse(DefaultResponse defaultResponse)
        {
            this.Status = defaultResponse.Status;
            this.Message = defaultResponse.Message;
            this.Done = defaultResponse.Done;
        }
    }
}
