using HttpClientService.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace HttpClientService.Test
{
    public class HttpResponseDeserializationExtensionsShould
    {
        [Fact]
        public async Task DeserializeToSpecifiedReferenceType()
        {


            // arrange
            var testContent = new TestClass
            {
                A = "test",
                B = 1
            };
            var sut = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(testContent))
            };

            // act
            var result = await sut.GetDeserializedContentAsync<TestClass>();

            // assert
            Assert.NotNull(result);
            Assert.Equal(result, testContent);
        }

        [Fact]
        public async Task ThrowExceptionIfResponseNotSuccessful()
        {
            // arrange
            var testContent = new TestClass
            {
                A = "test",
                B = 1
            };
            var sut = new HttpResponseMessage
            {
                // note: you would normally not get content if unsuccesful, but this is necessary to make sure it fails for the right reason
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Content = new StringContent(JsonSerializer.Serialize(testContent))
            };

            // act
            try
            {
                var result = await sut.GetDeserializedContentAsync<TestClass>();

                // assert
                Assert.True(false); // if we get here, it has failed the test; it is supposed to throw an exception
            }
            catch (Exception exception)
            {
                Assert.IsType<ArgumentException>(exception);
            }
        }

        [Fact]
        public async Task ThrowExceptionIfContentIsNull()
        {
            // arrange
            var sut = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = null
            };

            // act
            try
            {
                var result = await sut.GetDeserializedContentAsync<TestClass>();

                // assert
                Assert.True(false); // if we get here, it has failed the test; it is supposed to throw an exception
            }
            catch (Exception exception)
            {
                Assert.IsType<NullContentException>(exception);
            }
        }

        [Fact]
        public async Task ThrowExceptionIfContentIsEmpty()
        {
            // arrange
            var sut = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(String.Empty)
            };

            // act
            try
            {
                var result = await sut.GetDeserializedContentAsync<TestClass>();

                // assert
                Assert.True(false); // if we get here, it has failed the test; it is supposed to throw an exception
            }
            catch (Exception exception)
            {
                Assert.IsType<EmptyContentException>(exception);
            }
        }
    }
}
