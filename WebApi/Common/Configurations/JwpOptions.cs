namespace WebApi.Common.Configurations
{
    public class JwtOptions
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiredMin { get; set; }
    }

}