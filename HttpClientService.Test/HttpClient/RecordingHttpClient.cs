using System;
using System.Net.Http;
using System.Text.Json;
using System.Linq;

namespace HttpClientService.Test.Helpers
{
    public class RecordingHttpClient : HttpClient
    {
        private MockHttpMessageHandler MockHandler { get; set; }

        public RecordingHttpClient(MockHttpMessageHandler handler)
            : base(handler)
        {
            this.MockHandler = handler;
        }

        public bool CalledDestination(string uri)
        {
            return this.CalledDestination(new Uri(uri));
        }

        public bool CalledDestination(Uri uri)
        {
            return this.MockHandler.Requests.Any(x => x.RequestUri.AbsoluteUri == uri.AbsoluteUri);
        }

        public bool CalledDestinationWith(string uri, object content)
        {
            return this.CalledDestinationWith(new Uri(uri), content);
        }

        public bool CalledDestinationWith(Uri uri, object content)
        {
            return this.MockHandler.Requests.Any(x =>
            {
                var uriMatch = x.RequestUri.AbsoluteUri == uri.AbsoluteUri;
                var stringContent = new StringContent(JsonSerializer.Serialize(content));
                var contentMatch = x.Content == content;

                return uriMatch && contentMatch;
            });
        }
    }
}
