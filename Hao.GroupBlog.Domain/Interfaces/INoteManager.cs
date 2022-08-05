using Hao.GroupBlog.Domain.Models;
using Hao.GroupBlog.Domain.Paging;

namespace Hao.GroupBlog.Domain.Interfaces
{
    public interface INoteManager
    {
        public Task<bool> IsOpen(string id);

        public Task<ResponseResult<bool>> Add(NoteM model);

        public Task<ResponseResult<bool>> Update(NoteM model);

        public Task<ResponseResult<bool>> AddProfile(string id, string profileName);

        public Task<ResponseResult<bool>> Hit(string id);

        public Task<ResponseResult<bool>> Delete(string id);

        public Task<ResponsePagingResult<NoteM>> GetMyList(PagingParameter<string> parameter);

        public Task<ResponseResult<bool>> ToColumn(string id, string columnId);



        public Task<ResponseResult<bool>> Favorite(string id, string? columnId);

        public Task<ResponseResult<bool>> CancelFavorite(string id);

        public Task<ResponseResult<NoteContentM>> GetContent(string id);

        public Task<ResponseResult<bool>> SaveContent(NoteContentM model);

        public Task<ResponseResult<NoteContentM>> GetOpenedContent(string id);

        public Task<ResponsePagingResult<NoteM>> GetFavoriteList(PagingParameter<string> parameter);

        public Task<ResponsePagingResult<NoteM>> GetOpenedList(PagingParameter<string> parameter);
    }
}
