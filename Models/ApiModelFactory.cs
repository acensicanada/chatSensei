namespace ChatSensei.Models
{
    public static class ApiModelFactory
    {
        public static AbstractApiModel createApiModel(string inputMessage)
        {
            return inputMessage switch
            {
                string s when s.StartsWith("!image") => new ImageApiModel(),
                _ => new TextApiModel(),
            };
        }
    }
}
