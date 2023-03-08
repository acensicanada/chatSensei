using ChatSensei.Models.Chat;

namespace ChatSensei.Models.ApiModel
{
    public abstract class AbstractApiModel
    {
        protected string BaseUrl = "https://api.openai.com/v1/";
        public abstract string Url { get; }
        public abstract ApiModelType Type { get; }
    }
}
