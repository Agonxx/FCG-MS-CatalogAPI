using CatalogAPI.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CatalogAPI.Api.Controllers
{
    public class RoleAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public RoleAuthorize(string roles)
        {
            _roles = roles.Split(',');
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var role = context.HttpContext.User.FindFirst(nameof(InfoToken.Nivel))?.Value;

            if (!_roles.Contains(role))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
