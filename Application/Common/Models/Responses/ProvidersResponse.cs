using System.Collections.Generic;

namespace Application.Common.Models.Responses
{
    public class ProvidersResponse : DefaultResponse
    {
        public IList<string>? Providers { get; set; }

        public ProvidersResponse() { }

        public ProvidersResponse(DefaultResponse defaultResponse)
        {
            this.Status = defaultResponse.Status;
            this.Message = defaultResponse.Message;
            this.Done = defaultResponse.Done;
        }
    }
}
