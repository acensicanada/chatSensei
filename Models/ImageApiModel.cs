using Newtonsoft.Json;

namespace ChatSensei.Models
{
    public class ImageApiModel : AbstractApiModel
    {
        public override string url => "images/generations";

        public override string GetBody(string message)
        {
            var requestData = new
            {
                prompt = message.Substring(7)
            };
            return JsonConvert.SerializeObject(requestData);
        }
    }
}
