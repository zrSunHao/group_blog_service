using Hao.GroupBlog.Common.Enums;
using Hao.GroupBlog.Manager.Basic;
using Hao.GroupBlog.Manager.Implements;
using Hao.GroupBlog.Persistence.Entities;
using Hao.GroupBlog.Web.Attributes;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Hao.GroupBlog.Web.Middlewares
{
    public class PrivilegeMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<PrivilegeMiddleware> _logger;
        private IConfiguration _configuration;

        public PrivilegeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context,
            ILogger<PrivilegeMiddleware> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            var url = context.Request.Path.Value;
            if (string.IsNullOrEmpty(url) || !url.ToLower().StartsWith("/api"))
            {
                await _next(context);
            }
            else
            {
                try
                {
                    var result = await this.CheckPrivilege(context);
                    if (result) await _next.Invoke(context);
                    else
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("You do not have permission to access the requested data or object!");
                    }
                }
                catch (MyUnauthorizedException e)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync(e.Message);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    context.Response.Clear();
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync(e.Message);
                }
            }
        }

        private async Task<bool> CheckPrivilege(HttpContext context)
        {
            try
            {
                var allow = this.AllowAnonymous(context);
                if (allow) return true;

                PrivilegeManager? manager = context.RequestServices.GetService<PrivilegeManager>();
                if (manager == null) throw new Exception("privilege manager not instance!");
                UserLastLoginRecord record = await this.GetLoginRecord(context, manager);
                context.Items.Add(nameof(UserLastLoginRecord), record);

                if (record.Role == RoleType.super_manager) return true;

                var types = GetRoles(context);
                if (types.Any()) return true;
                return types.Any(x => x == record.Role);
            }
            catch (MyUnauthorizedException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        private bool AllowAnonymous(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var myAttribute = endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>();
                if (myAttribute != null) return true;
            }
            return false;
        }

        private RoleType[] GetRoles(HttpContext context)
        {
            RoleType[] types = new RoleType[3];
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var myAttribute = endpoint.Metadata.GetMetadata<MyRoleAttribute>();
                if (myAttribute != null)
                {
                    types = myAttribute.Types;
                }
            }
            return types;
        }

        private async Task<UserLastLoginRecord> GetLoginRecord(HttpContext context, PrivilegeManager manager)
        {
            Guid loginId = Guid.Empty;
            var sid = context.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value;
            return await manager.GetLoginRecord(sid);
        }
    }
}
