namespace CatalogAPI.Domain.DTOs
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public int ExpiracaoHoras { get; set; }
    }
}
