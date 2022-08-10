using Hao.GroupBlog.Domain.Models;
using Hao.GroupBlog.Domain.Paging;

namespace Hao.GroupBlog.Domain.Interfaces
{
    public interface ISeriesManager
    {
        public Task<ResponseResult<DomainM>> AddDomain(DomainM model);

        public Task<ResponseResult<bool>> UpdateDomain(DomainM model);

        public Task<ResponseResult<bool>> DeleteDomain(string id);

        public Task<ResponsePagingResult<DomainM>> GetDomainList();

        public Task<ResponseResult<bool>> SortDomain(List<OptionItem<int>> sequnces);

        public Task<ResponsePagingResult<OptionItem<string>>> GetDomainItems();


        public Task<ResponseResult<TopicM>> AddTopic(TopicM model);

        public Task<ResponseResult<bool>> UpdateTopic(TopicM model);

        public Task<ResponseResult<bool>> DeleteTopic(string id);

        public Task<ResponsePagingResult<OptionItem<string>>> GetTopicItems(string domainId);

        public Task<ResponseResult<bool>> SortTopic(SequnceM model);

        public Task<ResponseResult<bool>> AddTopicLogo(string id, string logo);


        public Task<ResponseResult<ColumnM>> AddColumn(ColumnM model);

        public Task<ResponseResult<bool>> UpdateColumn(ColumnM model);

        public Task<ResponseResult<bool>> DeleteColumn(string id);

        public Task<ResponsePagingResult<ColumnM>> GetColumnList(string topicId);

        public Task<ResponseResult<bool>> SortColumn(SequnceM model);

        public Task<ResponseResult<bool>> AddColumnLogo(string id, string logo);

        public Task<ResponsePagingResult<OptionItem<string>>> GetColumnItems(string topicId);


        public Task<ResponseResult<ColumnM>> AddFavoriteColumn(ColumnM model);

        public Task<ResponsePagingResult<ColumnM>> GetFavoriteColumnList();

        public Task<ResponsePagingResult<OptionItem<string>>> GetFavoriteColumnItems();

        public Task<ResponseResult<bool>> SortFavoriteColumn(SequnceM model);

        public Task<ResponseResult<bool>> DeleteFavoriteColumn(string id);
    }
}
