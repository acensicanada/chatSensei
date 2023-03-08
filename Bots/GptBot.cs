// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with EchoBot .NET Template version v4.17.1

using ChatSensei.Models.ApiModel;
using ChatSensei.Models.Chat;
using ChatSensei.Models.Client;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace chatSensei.Bots
{
    public class GptBot : ActivityHandler
    {
        private readonly IImageAIClient imageClient;
        private readonly ITextAIClient textClient;
        private readonly static Dictionary<string, ChatContext> conversationContext = new Dictionary<string, ChatContext>();

        public GptBot(IImageAIClient imageClient, ITextAIClient textClient)
        {
            this.imageClient = imageClient;
            this.textClient = textClient;
        }

        public string CheckConversationID(ITurnContext<IMessageActivity> turnContext)
        {
            string conversationId = turnContext.Activity.Conversation.Id;

            if (!conversationContext.ContainsKey(conversationId))
            {
                // Create a new ConversationState for this conversation if one does not already exist
                conversationContext[conversationId] = new ChatContext();
            }

            return conversationId;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await base.OnMessageActivityAsync(turnContext, cancellationToken);

            string userText = turnContext.Activity.RemoveRecipientMention();

            IMessageActivity finalResponse = await GetChatGPTResponseAsync(userText, turnContext);
            await turnContext.SendActivityAsync(finalResponse, cancellationToken);
        }

        private async Task<IMessageActivity> GetChatGPTResponseAsync(string inputMessage, ITurnContext<IMessageActivity> turnContext)
        {
            var apiModel = ApiModelFactory.CreateApiModel(inputMessage);

            IMessageActivity responseMessage;
            if (apiModel.Type == ApiModelType.Image)
            {
                responseMessage = await imageClient.CallOpenAIApi(apiModel as ImageApiModel, inputMessage);
            }
            else
            {
                string conversationID = CheckConversationID(turnContext);
                responseMessage = await textClient.CallOpenAIApi(apiModel as TextApiModel, conversationContext[conversationID], inputMessage);
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
