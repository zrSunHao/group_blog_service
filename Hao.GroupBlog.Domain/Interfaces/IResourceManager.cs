using Hao.GroupBlog.Domain.Models;
using Hao.GroupBlog.Domain.Paging;

namespace Hao.GroupBlog.Domain.Interfaces
{
    public interface IResourceManager
    {
        public Task<ResponseResult<bool>> Save(ResourceM model);

        public Task<ResponseResult<ResourceM>> GetByCode(string code);

        public string GetNewCode();

        public string BuilderFileUrl(string? fileName);
    }
}
