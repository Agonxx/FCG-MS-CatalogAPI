namespace CatalogAPI.Domain.DTOs
{
    public class JogoDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NomeComprador { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public decimal PrecoPago { get; set; }
        public DateTime AdquiridoEm { get; set; }
        public DateTime CadastradoEm { get; set; }
    }
}
