using CatalogAPI.Domain;
using CatalogAPI.Domain.DTOs;
using CatalogAPI.Domain.Extensions;

namespace CatalogAPI.Api.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtMiddleware> _logger;

        public JwtMiddleware(RequestDelegate next, ILogger<JwtMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, InfoToken _infoToken)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                _infoToken.Id = int.Parse(context.User.FindFirst(nameof(InfoToken.Id)).Value);
                _infoToken.Nome = context.User.FindFirst(nameof(InfoToken.Nome)).Value;
                _infoToken.Email = context.User.FindFirst(nameof(InfoToken.Email)).Value;
                _infoToken.Nivel = EnumExtensions.ToEnum<ETipoUsuario>(context.User.FindFirst(nameof(InfoToken.Nivel)).Value);
                _infoToken.CadastradoEm = DateTime.Parse(context.User.FindFirst(nameof(InfoToken.CadastradoEm)).Value);
            }

            await _next(context);
        }
    }
}
