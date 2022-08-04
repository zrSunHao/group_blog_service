using Hao.GroupBlog.Domain.Models;
using Hao.GroupBlog.Domain.Paging;

namespace Hao.GroupBlog.Domain.Interfaces
{
    public interface ISeriesManager
    {
        public Task<ResponseResult<bool>> AddDomain(DomainM model);

        public Task<ResponseResult<bool>> UpdateDomain(DomainM model);

        public Task<ResponseResult<bool>> DeleteDomain(string id);

        public Task<ResponsePagingResult<DomainM>> GetDomainList();

        public Task<ResponseResult<bool>> SortDomain(SequnceM model);


        public Task<ResponseResult<bool>> AddTopic(TopicM model);

        public Task<ResponseResult<bool>> UpdateTopic(TopicM model);

        public Task<ResponseResult<bool>> DeleteTopic(string id);

        public Task<ResponsePagingResult<TopicM>> GetTopicList(string domainId);

        public Task<ResponseResult<bool>> SortTopic(SequnceM model);


        public Task<ResponseResult<bool>> AddColumn(ColumnM model);

        public Task<ResponseResult<bool>> UpdateColumn(ColumnM model);

        public Task<ResponseResult<bool>> DeleteColumnc(string id);

        public Task<ResponsePagingResult<TopicM>> GetColumnList(string topicId);

        public Task<ResponseResult<bool>> SortColumn(SequnceM model);
    }
}
