using System.ComponentModel;

namespace CatalogAPI.Domain
{
    public enum ETipoUsuario
    {
        [Description("Usuário")]
        Usuario = 1,
        [Description("Administrador")]
        Administrador = 2,
    }
}
