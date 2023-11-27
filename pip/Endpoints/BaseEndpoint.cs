using GPTAPIS.Net.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GPTAPIS.Endpoints
{
    public abstract class BaseEndpoint
    {
        protected abstract string Address { get; set; }
        protected abstract HttpClient Client { get; set; }
        protected abstract APIService Service { get; set; }
        protected abstract bool EnableDebug { get; }

        public abstract Task<ChatResponse> SendRequest(RequestForm request);
        public abstract Task<ChatResponse> SendRequestAsync(RequestForm request);
        public abstract Task<ChatResponse> SendStreamRequestAsync(RequestForm request, Action<ChatResponse> responseHandler);
    }
}
