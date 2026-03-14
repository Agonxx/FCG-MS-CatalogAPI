using CatalogAPI.Domain.Constants;
using CatalogAPI.Domain.DTOs;
using CatalogAPI.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Api.Controllers
{
    public class BibliotecaController : BaseController
    {
        private readonly IBibliotecaService _service;

        public BibliotecaController(IHttpContextAccessor httpContextAccessor,
                                    IBibliotecaService bibliotecaService,
                                    InfoToken infoToken) : base(httpContextAccessor, infoToken)
        {
            _service = bibliotecaService;
        }

        [HttpGet(ItemBibliotecaApi.GetMyGames)]
        [RoleAuthorize(Roles.UsuarioAccess)]
        public async Task<IActionResult> GetMyGames()
        {
            var obj = await _service.GetMyGamesAsync();
            return Ok(obj);
        }

        [HttpGet(ItemBibliotecaApi.GetUserGamesById)]
        [RoleAuthorize(Roles.AdminAccess)]
        public async Task<IActionResult> GetUserGamesById([FromQuery] int id)
        {
            var obj = await _service.GetUserGamesByIdAsync(id);
            return Ok(obj);
        }

        [HttpPost(ItemBibliotecaApi.BuyGame)]
        [RoleAuthorize(Roles.UsuarioAccess)]
        public async Task<IActionResult> BuyGame([FromQuery] int id)
        {
            var obj = await _service.BuyGameAsync(id);
            return Ok(obj);
        }
    }
}
