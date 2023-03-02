// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with EchoBot .NET Template version v4.17.1

using chatSensei;
using ChatSensei.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chatSensei.Bots
{
    public class GptBot : ActivityHandler
    {

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await base.OnMessageActivityAsync(turnContext, cancellationToken);

            string userText = turnContext.Activity.RemoveRecipientMention();

            Console.WriteLine("Meesage received :" + userText);

            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
            string openAIKey = config["openAIKey"];
            Console.WriteLine(openAIKey);

            IMessageActivity finalResponse = await GetChatGPTResponseAsync(userText, openAIKey);
            await turnContext.SendActivityAsync(finalResponse, cancellationToken);
        }

        private async Task<IMessageActivity> GetChatGPTResponseAsync(string inputMessage, string openAIKey)
        {
            IMessageActivity responseMessage = null;
            var apiModel = ApiModelFactory.CreateApiModel(inputMessage);
            string url = $"https://api.openai.com/v1/{apiModel.url}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAIKey}");
                string body = apiModel.GetBody(inputMessage);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(url, content);
                Console.WriteLine(result.StatusCode);
                if (result.IsSuccessStatusCode)
                {
                    var responseContent = await result.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(responseContent);

                    if (apiModel is ImageApiModel)
                    {
                        var imageUrl = data.data[0].url;
                        var attachment = new Attachment
                        {
                            Name = "image.png",
                            ContentType = "image/png",
                            ContentUrl = imageUrl
                        };

                        responseMessage = MessageFactory.Attachment(attachment);
                    }
                    else
                    {
                        string textResponse = data.choices[0].text;
                        responseMessage = MessageFactory.Text(textResponse);
                    }
                }
            }
            return responseMessage;
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Bonjour ! Comment puis-je vous aider?";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
