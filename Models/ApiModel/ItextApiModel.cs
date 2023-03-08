using ChatSensei.Models.Chat;

namespace ChatSensei.Models.ApiModel
{
    public interface ItextApiModel
    {
        public string GetBody(ChatContext context, string message);
    }
}
