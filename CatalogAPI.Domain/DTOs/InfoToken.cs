namespace CatalogAPI.Domain.DTOs
{
    public class InfoToken
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public ETipoUsuario Nivel { get; set; }
        public DateTime CadastradoEm { get; set; }
    }
}
