using Hao.GroupBlog.Domain.Models;
using Hao.GroupBlog.Domain.Paging;

namespace Hao.GroupBlog.Domain.Interfaces
{
    public interface IMemberManager
    {
        public Task<ResponseResult<bool>> Register(LoginM model);

        public Task<ResponseResult<LoginRes>> Login(LoginM model);

        public Task<ResponseResult<bool>> Logout();

        public Task<ResponseResult<bool>> Update(MemberM model);

        public Task<ResponseResult<bool>> ManagerResetPsd(ResetPsdM model);

        public Task<ResponseResult<bool>> MyselfResetPsd(ResetPsdM model);

        public Task<ResponsePagingResult<MemberM>> GetList(PagingParameter<MemberFilter> parameter);
    }
}
