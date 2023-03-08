using ChatSensei.Models.ApiModel;
using ChatSensei.Models.Chat;
using Microsoft.Bot.Schema;
using System.Threading.Tasks;

namespace ChatSensei.Models.Client
{
    public interface IImageAIClient
    {
        public Task<IMessageActivity> CallOpenAIApi(ImageApiModel apiModel, string inputMessage);
    }
}
