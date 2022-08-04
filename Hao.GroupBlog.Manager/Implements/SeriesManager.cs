using AutoMapper;
using Hao.GroupBlog.Domain.Interfaces;
using Hao.GroupBlog.Domain.Models;
using Hao.GroupBlog.Domain.Paging;
using Hao.GroupBlog.Manager.Basic;
using Hao.GroupBlog.Persistence.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hao.GroupBlog.Manager.Implements
{
    public class SeriesManager : BaseManager, ISeriesManager
    {
        private readonly ILogger _logger;

        public SeriesManager(GbDbContext dbContext,
            IMapper mapper,
            ILogger<SeriesManager> logger,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
            : base(dbContext, mapper, configuration, httpContextAccessor)
        {
            _logger = logger;
        }

        public async Task<ResponseResult<bool>> AddDomain(DomainM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"添加领域【{model.Name}】失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> UpdateDomain(DomainM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"更新领域【{model.Name}】失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> DeleteDomain(string id)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"删除领域【{id}】失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<DomainM>> GetDomainList()
        {
            var res = new ResponsePagingResult<DomainM>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"获取领域列表失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> SortDomain(SequnceM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"领域排序失败！");
            }
            return res;
        }


        public async Task<ResponseResult<bool>> AddTopic(TopicM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"添加主题【{model.Name}】失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> UpdateTopic(TopicM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"更新主题【{model.Name}】失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> DeleteTopic(string id)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"删除主题【{id}】失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<TopicM>> GetTopicList(string domainId)
        {
            var res = new ResponsePagingResult<TopicM>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"获取领域【{domainId}】下的主题列表失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> SortTopic(SequnceM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"领域【{model.DropGroupKey}】下的主题排序失败！");
            }
            return res;
        }


        public async Task<ResponseResult<bool>> AddColumn(ColumnM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"添加专栏【{model.Name}】失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> UpdateColumn(ColumnM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"更新专栏【{model.Name}】失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> DeleteColumnc(string id)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"删除专栏【{id}】失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<TopicM>> GetColumnList(string topicId)
        {
            var res = new ResponsePagingResult<TopicM>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"获取主题【{topicId}】下的专栏列表失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> SortColumn(SequnceM model)
        {
            var res = new ResponseResult<bool>();
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"主题【{model.DropGroupKey}】下的专栏排序失败！");
            }
            return res;
        }
    }
}
