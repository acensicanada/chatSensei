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
    public sealed class ImageAIClient : OpenAiClient, IImageAIClient
    {
        public async Task<IMessageActivity> CallOpenAIApi(ImageApiModel apiModel, string inputMessage)
        {
            string body = apiModel.GetBody(inputMessage);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var result = await HttpClient.PostAsync(apiModel.Url, content);

            var responseContent = await result.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(responseContent);

            var imageUrl = data.data[0].url;
            var attachment = new Attachment
            {
                Name = "image.png",
                ContentType = "image/png",
                ContentUrl = imageUrl
            };

            return MessageFactory.Attachment(attachment);
        }
    }
}
