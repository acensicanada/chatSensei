namespace ChatSensei.Models.ApiModel
{
    public static class ApiModelFactory
    {
        public static AbstractApiModel CreateApiModel(string inputMessage)
        {
            return inputMessage switch
            {
                string s when s.StartsWith("!image") => new ImageApiModel(),
                _ => new TextApiModel(),
            };
        }
    }
}
