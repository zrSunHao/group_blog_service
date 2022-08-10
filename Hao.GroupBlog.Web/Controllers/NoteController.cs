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
        public async Task<ResponseResult<NoteM>> Add(NoteM model)
        {
            return await _manager.Add(model);
        }

        [HttpPatch("Update")]
        public async Task<ResponseResult<bool>> Update(NoteM model)
        {
            return await _manager.Update(model);
        }

        [HttpPatch("Open")]
        public async Task<ResponseResult<bool>> Open(string id, bool opened)
        {
            return await _manager.Open(id, opened);
        }

        [HttpPatch("AddProfile")]
        public async Task<ResponseResult<bool>> AddProfile(string id, string profileName)
        {
            return await _manager.AddProfile(id, profileName);
        }

        [HttpPatch("Hit")]
        public async Task<ResponseResult<bool>> Hit(string id)
        {
            return await _manager.Hit(id);
        }

        [HttpPatch("ToColumn")]
        public async Task<ResponseResult<bool>> ToColumn(string id, string columnId)
        {
            return await _manager.ToColumn(id, columnId);
        }

        [HttpDelete("Delete")]
        public async Task<ResponseResult<bool>> Delete(string id)
        {
            return await _manager.Delete(id);
        }

        [HttpGet("GetMyList")]
        public async Task<ResponsePagingResult<NoteM>> GetMyList(string columnId)
        {
            return await _manager.GetMyList(columnId);
        }

        [HttpPatch("SortMyNotes")]
        public async Task<ResponseResult<bool>> SortMyNotes(SequnceM model)
        {
            return await _manager.SortMyNotes(model);
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

        [HttpPatch("SaveContent")]
        public async Task<ResponseResult<bool>> SaveContent(NoteContentM model)
        {
            return await _manager.SaveContent(model);
        }

        [HttpGet("GetOpenedContent")]
        [AllowAnonymous]
        public async Task<ResponseResult<NoteContentM>> GetOpenedContent(string id)
        {
            return await _manager.GetOpenedContent(id);
        }

        [HttpGet("GetFavoriteList")]
        public async Task<ResponsePagingResult<NoteM>> GetFavoriteList(string columnId)
        {
            return await _manager.GetFavoriteList(columnId);
        }

        [HttpPost("GetOpenedList")]
        public async Task<ResponsePagingResult<NoteM>> GetOpenedList(PagingParameter<string> parameter)
        {
            return await _manager.GetOpenedList(parameter);
        }
    }
}
