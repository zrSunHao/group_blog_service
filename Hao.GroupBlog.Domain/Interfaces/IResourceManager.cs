using Hao.GroupBlog.Domain.Models;
using Hao.GroupBlog.Domain.Paging;

namespace Hao.GroupBlog.Domain.Interfaces
{
    public interface IResourceManager
    {
        public Task<ResponseResult<bool>> Save(FileM model);

        public Task<ResponseResult<FileM>> GetByCode(string code);

        public Task<ResponsePagingResult<ResourceM>> GetList(PagingParameter<ResourceFilter> parameter);

        public string GetNewCode();
    }
}
