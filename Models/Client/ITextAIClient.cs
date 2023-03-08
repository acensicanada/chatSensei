using ChatSensei.Models.ApiModel;
using ChatSensei.Models.Chat;
using Microsoft.Bot.Schema;
using System.Threading.Tasks;

namespace ChatSensei.Models.Client
{
    public interface ITextAIClient
    {
        public Task<IMessageActivity> CallOpenAIApi(TextApiModel apiModel, ChatContext chatContext, string inputMessage);
    }
}
