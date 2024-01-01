using Domain.Entities;

namespace Application.Common.Models.Responses
{
    public class FileResponse : DefaultResponse
    {
        public File? File { get; set; }

        public FileResponse() { }

        public FileResponse(DefaultResponse defaultResponse)
        {
            this.Status = defaultResponse.Status;
            this.Message = defaultResponse.Message;
            this.Done = defaultResponse.Done;
        }
    }
}
