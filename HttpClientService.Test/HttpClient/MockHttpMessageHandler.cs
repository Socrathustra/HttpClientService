using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientService.Test.Helpers
{

    public class MockHttpMessageHandler : HttpMessageHandler
    {
        public ICollection<HttpRequestMessage> Requests { get; private set; } = new List<HttpRequestMessage>();
        public IList<string> Responses { get; set; }
        private int CurrentResponseIndex = 0;
        public bool WillLoopResponses = false;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously - justification: replacing async code; async is required for method header
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            this.Requests.Add(request);

            var response = this.Responses?.Count > 0 ? this.Responses[this.CurrentResponseIndex] : null;
            if (this.WillLoopResponses == true)
            {
                // iterate but loop back to the beginning
                var modulusBy = this.Responses?.Count == 0 ? 1 : this.Responses.Count;
                this.CurrentResponseIndex = (this.CurrentResponseIndex + 1) % modulusBy;
            }
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(response)
            };
        }
    }
}
