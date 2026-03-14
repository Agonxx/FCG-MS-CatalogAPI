using CatalogAPI.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public abstract class BaseController : ControllerBase
    {
        public readonly InfoToken _infoToken;

        public BaseController(IHttpContextAccessor httpContextAccessor, InfoToken infoToken)
        {
            _infoToken = infoToken;
        }
    }
}
