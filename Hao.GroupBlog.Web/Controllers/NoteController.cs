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
    public class NoteController : ControllerBase
    {
        private readonly INoteManager _manager;

        public NoteController(INoteManager manager)
        {
            _manager = manager;
        }

        [HttpPost("Add")]
        public async Task<ResponseResult<bool>> Add(NoteM model)
        {
            return await _manager.Add(model);
        }

        [HttpPatch("Update")]
        public async Task<ResponseResult<bool>> Update(NoteM model)
        {
            return await _manager.Update(model);
        }

        [HttpDelete("Delete")]
        public async Task<ResponseResult<bool>> Delete(string id)
        {
            return await _manager.Delete(id);
        }

        [HttpPost("GetList")]
        public async Task<ResponsePagingResult<NoteM>> GetList(PagingParameter<string> parameter)
        {
            return await _manager.GetList(parameter);
        }

        [HttpPatch("ToColumn")]
        public async Task<ResponseResult<bool>> ToColumn(string id, string columnId)
        {
            return await _manager.ToColumn(id, columnId);
        }

        [HttpPatch("Favorite")]
        public async Task<ResponseResult<bool>> Favorite(string id, string? columnId)
        {
            return await _manager.Favorite(id, columnId);
        }

        [HttpDelete("CancelFavorite")]
        public async Task<ResponseResult<bool>> CancelFavorite(string id)
        {
            return await _manager.CancelFavorite(id);
        }

        [HttpGet("GetContent")]
        public async Task<ResponseResult<NoteContentM>> GetContent(string id)
        {
            return await _manager.GetContent(id);
        }

        [HttpPost("SaveContent")]
        public async Task<ResponseResult<bool>> SaveContent(NoteContentM model)
        {
            return await _manager.SaveContent(model);
        }

        [HttpGet("GetOpenedContent")]
        public async Task<ResponseResult<NoteContentM>> GetOpenedContent(string id)
        {
            return await _manager.GetOpenedContent(id);
        }
    }
}
