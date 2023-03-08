using System.Collections.Generic;

namespace ChatSensei.Models.Chat
{
    public class ChatContext
    {
        private readonly Queue<ChatMessage> Messages;
        public int SumToken { get; private set; }

        public ChatContext()
        {
            Messages = new Queue<ChatMessage>();
            SumToken = 0;
        }

        public void AddMessage(ChatMessage message) 
        {
            Messages.Enqueue(message);
            SumToken += message.TokenNumber;
        }

        public void RemoveMessage()
        {
            ChatMessage removedMessage = Messages.Dequeue();
            SumToken -= removedMessage.TokenNumber;
        }

        public override string ToString()
        {
            return string.Join("\n", Messages);
        }
    }
}
