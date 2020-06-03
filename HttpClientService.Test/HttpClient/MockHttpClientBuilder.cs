using System.Collections.Generic;
using System.Text.Json;

namespace HttpClientService.Test.Helpers
{
    // this class is intentionally over-engineered compared to the library to demonstrate that I also put effort into making tools for testing
    public class MockHttpClientBuilder
    {
        private IList<string> Responses { get; set; }
        private bool WillLoopResponses = false;

        private MockHttpClientBuilder()
        {
            this.Responses = new List<string>();
        }

        public MockHttpClientBuilder WithResponse(object response)
        {
            var jsonString = JsonSerializer.Serialize(response);
            return this.WithResponse(jsonString);
        }

        public MockHttpClientBuilder WithResponse(string response)
        {
            this.Responses.Add(response);
            return this;
        }

        public MockHttpClientBuilder WithResponses(IList<object> responses)
        {
            foreach (var response in responses)
            {
                this.WithResponse(response);
            }

            return this;
        }

        public MockHttpClientBuilder WithResponses(IList<string> responses)
        {
            foreach (var response in responses)
            {
                this.WithResponse(response);
            }
            return this;
        }

        public MockHttpClientBuilder WithLoopingBehavior(bool willLoopResponses)
        {
            this.WillLoopResponses = willLoopResponses;
            return this;
        }

        public static MockHttpClientBuilder GetNew()
        {
            return new MockHttpClientBuilder();
        }

        public RecordingHttpClient Build()
        {
            return new RecordingHttpClient(
                new MockHttpMessageHandler
                {
                    Responses = this.Responses,
                    WillLoopResponses = this.WillLoopResponses
                });
        }
    }
}
