using ChatSensei.Models.ApiModel;
using ChatSensei.Models.Chat;
using Microsoft.Bot.Schema;
using System.Net.Http;
using System.Text;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Bot.Builder;

namespace ChatSensei.Models.Client
{
    public sealed class TextAIClient : OpenAiClient, ITextAIClient
    {
        public async Task<IMessageActivity> CallOpenAIApi(TextApiModel apiModel, ChatContext chatContext, string inputMessage)
        {
            string body = apiModel.GetBody(chatContext, inputMessage);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var result = await HttpClient.PostAsync(apiModel.Url, content);

            var responseContent = await result.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(responseContent);

            string textResponse = data.choices[0].text;
            int promptToken = data.usage.prompt_tokens;
            int reponseToken = data.usage.completion_tokens;

            chatContext.AddMessage(new ChatMessage(inputMessage, promptToken));
            chatContext.AddMessage(new ChatMessage(textResponse, reponseToken));

            return MessageFactory.Text(textResponse);
        }
    }
}
