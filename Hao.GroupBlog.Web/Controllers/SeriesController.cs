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
    public class SeriesController : ControllerBase
    {
        private readonly ISeriesManager _manager;

        public SeriesController(ISeriesManager manager)
        {
            _manager = manager;
        }

        [HttpPost("AddDomain")]
        public async Task<ResponseResult<DomainM>> AddDomain(DomainM model)
        {
            return await _manager.AddDomain(model);
        }

        [HttpPatch("UpdateDomain")]
        public async Task<ResponseResult<bool>> UpdateDomain(DomainM model)
        {
            return await _manager.UpdateDomain(model);
        }

        [HttpDelete("DeleteDomain")]
        public async Task<ResponseResult<bool>> DeleteDomain(string id)
        {
            return await _manager.DeleteDomain(id);
        }

        [HttpGet("GetDomainList")]
        public async Task<ResponsePagingResult<DomainM>> GetDomainList()
        {
            return await _manager.GetDomainList();
        }

        [HttpPatch("SortDomain")]
        public async Task<ResponseResult<bool>> SortDomain(List<OptionItem<int>> sequnces)
        {
            return await _manager.SortDomain(sequnces);
        }




        [HttpPost("AddTopic")]
        public async Task<ResponseResult<TopicM>> AddTopic(TopicM model)
        {
            return await _manager.AddTopic(model);
        }

        [HttpPatch("UpdateTopic")]
        public async Task<ResponseResult<bool>> UpdateTopic(TopicM model)
        {
            return await _manager.UpdateTopic(model);
        }

        [HttpDelete("DeleteTopic")]
        public async Task<ResponseResult<bool>> DeleteTopic(string id)
        {
            return await _manager.DeleteTopic(id);
        }

        [HttpGet("GetTopicList")]
        public async Task<ResponsePagingResult<TopicM>> GetTopicList(string domainId)
        {
            return await _manager.GetTopicList(domainId);
        }

        [HttpPatch("SortTopic")]
        public async Task<ResponseResult<bool>> SortTopic(SequnceM model)
        {
            return await _manager.SortTopic(model);
        }

        [HttpPatch("AddTopicLogo")]
        public async Task<ResponseResult<bool>> AddTopicLogo(string id, string logo)
        {
            return await _manager.AddTopicLogo(id, logo);
        }



        [HttpPost("AddColumn")]
        public async Task<ResponseResult<ColumnM>> AddColumn(ColumnM model)
        {
            return await _manager.AddColumn(model);
        }

        [HttpPatch("UpdateColumn")]
        public async Task<ResponseResult<bool>> UpdateColumn(ColumnM model)
        {
            return await _manager.UpdateColumn(model);
        }

        [HttpDelete("DeleteColumnc")]
        public async Task<ResponseResult<bool>> DeleteColumnc(string id)
        {
            return await _manager.DeleteColumnc(id);
        }

        [HttpGet("GetColumnList")]
        public async Task<ResponsePagingResult<ColumnM>> GetColumnList(string topicId)
        {
            return await _manager.GetColumnList(topicId);
        }

        [HttpPatch("SortColumn")]
        public async Task<ResponseResult<bool>> SortColumn(SequnceM model)
        {
            return await _manager.SortColumn(model);
        }

        [HttpPatch("AddColumnLogo")]
        public async Task<ResponseResult<bool>> AddColumnLogo(string id, string logo)
        {
            return await _manager.AddColumnLogo(id, logo);
        }
    }
}
