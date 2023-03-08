using ChatSensei.Models.Chat;
using Newtonsoft.Json;

namespace ChatSensei.Models.ApiModel
{
    public class TextApiModel : AbstractApiModel, ItextApiModel
    {
        private const int maxResponseToken = 2048;
        private const string initBot = "A chaque fois tu recevras tout l'historique de la conversation. Tu vas répondre toujours au dernier paragraphe.";

        public override string Url => $"{BaseUrl}engines/text-davinci-003/completions";

        public override ApiModelType Type => ApiModelType.Text;

        public string GetBody(ChatContext context, string message)
        {
            while (context.SumToken > maxResponseToken)
            {
                context.RemoveMessage();
            }

            string initBotAndContext = $"{initBot}\n{context}";

            var requestData = new
            {
                prompt = $"{initBotAndContext}\n{message}",
                max_tokens = maxResponseToken,
                temperature = 0.7
            };
            return JsonConvert.SerializeObject(requestData);
        }
    }
}
