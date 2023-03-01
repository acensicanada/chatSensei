using Newtonsoft.Json;

namespace ChatSensei.Models
{
    public class TextApiModel : AbstractApiModel
    {
        public override string url => "engines/text-davinci-003/completions";

        public override string GetBody(string message)
        {
            var requestData = new
            {
                prompt = message,
                max_tokens = 2048,
                temperature = 0.7
            };
            return JsonConvert.SerializeObject(requestData);
        }
    }
}
