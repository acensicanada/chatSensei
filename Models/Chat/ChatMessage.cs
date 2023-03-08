using System.Runtime.InteropServices;

namespace ChatSensei.Models.Chat
{
    public class ChatMessage
    {
        public string Content { get; }

        public int TokenNumber { get; }

        public ChatMessage(string content, int tokenNumber)
        {
            this.Content = content;
            this.TokenNumber = tokenNumber;
        }

        public override string ToString()
        {
            return Content;
        }
    }
}
