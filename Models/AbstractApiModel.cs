namespace ChatSensei.Models
{
    public abstract class AbstractApiModel
    {
        public abstract string url { get; }
        public abstract string GetBody(string message);
    }
}
