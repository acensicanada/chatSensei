using System;
using System.Net.Http;

namespace ChatSensei.Models.Client
{
    public abstract class OpenAiClient
    {
        protected HttpClient HttpClient { get; }

        public OpenAiClient()
        {
            HttpClient = new HttpClient();
            string openaiToken = Environment.GetEnvironmentVariable("OPENAI_TOKEN");
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {openaiToken}");
        }
    }
}
