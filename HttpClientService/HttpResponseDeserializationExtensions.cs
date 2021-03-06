﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpClientService
{
    public static class HttpResponseDeserializationExtensions
    {
        public static async Task<T> GetDeserializedContentAsync<T>(this HttpResponseMessage message, JsonSerializerOptions options = null)
        {
            if (message.IsSuccessStatusCode == true)
            {
                if (message.Content == null)
                {
                    throw new NullContentException($"{nameof(HttpResponseMessage)}.{nameof(message.Content)} is null and therefore cannot be deserialized to type {nameof(T)}");
                }
                var jsonContent = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (String.IsNullOrWhiteSpace(jsonContent) == true)
                {
                    throw new EmptyContentException($"The JSON content of {nameof(HttpResponseMessage)}.{nameof(message.Content)} is null or empty and therefore cannot be deserialized to type {nameof(T)}");
                }

                return JsonSerializer.Deserialize<T>(jsonContent, options);
            }

            throw new ArgumentException($"{nameof(HttpResponseMessage)}.{nameof(message.IsSuccessStatusCode)} is false, and therefore {nameof(HttpResponseMessage)}.{nameof(message.Content)} cannot be converted to type {nameof(T)}");
        }
    }

    public class NullContentException : ApplicationException
    {
        public NullContentException(string message)
            : base(message)
        {
        }
    }

    public class EmptyContentException : ApplicationException
    {
        public EmptyContentException(string message)
            : base(message)
        {
        }
    }
}
