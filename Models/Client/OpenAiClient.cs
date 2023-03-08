using ChatSensei.Models.ApiModel;
using Microsoft.Bot.Schema;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChatSensei.Models.Client
{
    public abstract class OpenAiClient
    {
        protected HttpClient HttpClient { get; }

        // TODO: Manage token properly

        public OpenAiClient()
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer ");
        }
    }
}
