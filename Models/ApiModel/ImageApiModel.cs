using ChatSensei.Models.Chat;
using Newtonsoft.Json;

namespace ChatSensei.Models.ApiModel
{
    public class ImageApiModel : AbstractApiModel, IImageApiModel
    {
        private const int StartIndex = 7;

        public override string Url => $"{BaseUrl}images/generations";

        public override ApiModelType Type => ApiModelType.Image;

        public string GetBody(string message)
        {
            var requestData = new
            {
                prompt = message[StartIndex..]
            };
            return JsonConvert.SerializeObject(requestData);
        }
    }
}
