using Hao.GroupBlog.Domain.Interfaces;
using Hao.GroupBlog.Domain.Models;
using Hao.GroupBlog.Domain.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hao.GroupBlog.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly IMemberManager _manager;

        public MemberController(IMemberManager manager)
        {
            _manager = manager;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ResponseResult<bool>> Register(LoginM model)
        {
            return await _manager.Register(model);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ResponseResult<LoginRes>> Login(LoginM model)
        {
            return await _manager.Login(model);
        }

        [HttpDelete("Logout")]
        public async Task<ResponseResult<bool>> Logout()
        {
            return await _manager.Logout();
        }

        [HttpPatch("Update")]
        public async Task<ResponseResult<bool>> Update(MemberM model)
        {
            return await _manager.Update(model);
        }

        [HttpPatch("ManagerResetPsd")]
        public async Task<ResponseResult<bool>> ManagerResetPsd(ResetPsdM model)
        {
            return await _manager.ManagerResetPsd(model);
        }

        [HttpPatch("MyselfResetPsd")]
        public async Task<ResponseResult<bool>> MyselfResetPsd(ResetPsdM model)
        {
            return await _manager.MyselfResetPsd(model);
        }

        [HttpPost("GetList")]
        public async Task<ResponsePagingResult<MemberM>> GetList(PagingParameter<MemberFilter> parameter)
        {
            return await _manager.GetList(parameter);
        }
    }
}
