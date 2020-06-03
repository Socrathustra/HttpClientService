using HttpClientService.Test.Helpers;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace HttpClientService.Test
{
    public class HttpClientServiceShould
    {
        [Fact]
        public async Task GetAndReturnSpecifiedType()
        {
            // arrange
            var testResponse = new TestClass { A = "test", B = 1 };

            var httpClient = MockHttpClientBuilder.GetNew()
                .WithResponse(testResponse)
                .Build();
            var sut = new HttpClientService(httpClient);

            // act
            var result = await sut.GetAsync<TestClass>("https://somewhere");

            // assert
            Assert.Equal(result, testResponse);
            Assert.True(httpClient.CalledDestination("https://somewhere"));
        }

        [Fact]
        public async Task GetAndReturnSpecifiedTypeWithQueryString()
        {
            // arrange
            var testResponse = new TestClass { A = "test", B = 1 };

            var httpClient = MockHttpClientBuilder.GetNew()
                .WithResponse(testResponse)
                .Build();
            var sut = new HttpClientService(httpClient);

            // act
            var result = await sut.GetAsync<TestClass>("https://somewhere", new Dictionary<string, string> { {"property1", "test" } });

            // assert
            Assert.Equal(result, testResponse);
            Assert.True(httpClient.CalledDestination(new Uri("https://somewhere/?property1=test")));
        }

        [Fact]
        public async Task PostAndReturnSpecifiedType()
        {
            // arrange
            var testResponse = new TestClass { A = "test", B = 1 };

            var httpClient = MockHttpClientBuilder.GetNew()
                .WithResponse(testResponse)
                .Build();
            var sut = new HttpClientService(httpClient);

            // act
            var result = await sut.PostAsync<TestClass>(new Uri("https://somewhere"), new object());

            // assert
            Assert.Equal(result, testResponse);
            Assert.True(httpClient.CalledDestination("https://somewhere"));
        }
    }
}
