// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with EchoBot .NET Template version v4.17.1

using chatSensei;
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

            // Get user input text
            string userText = turnContext.Activity.RemoveRecipientMention();

            Console.WriteLine("Meesage received :" + userText);

            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
            string openAIKey = config["openAIKey"];
            Console.WriteLine(openAIKey);

            string finalResponse = await GetChatGPTResponseAsync(userText, openAIKey);
            await turnContext.SendActivityAsync(MessageFactory.Text(finalResponse, finalResponse), cancellationToken);
        }

        private async Task<string> GetChatGPTResponseAsync(string input, string openAIKey)
        {
            string response = null;
            string url = "https://api.openai.com/v1/engines/text-davinci-003/completions";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAIKey}");
                var requestData = new
                {
                    prompt = input,
                    max_tokens = 2048,
                    temperature = 0.7
                };
                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(url, content);
                Console.WriteLine(result.StatusCode);
                if (result.IsSuccessStatusCode)
                {
                    var responseContent = await result.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(responseContent);
                    response = data.choices[0].text;
                }
            }
            return response;
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
