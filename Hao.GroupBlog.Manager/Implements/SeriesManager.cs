using AutoMapper;
using Hao.GroupBlog.Domain.Interfaces;
using Hao.GroupBlog.Domain.Models;
using Hao.GroupBlog.Domain.Paging;
using Hao.GroupBlog.Manager.Basic;
using Hao.GroupBlog.Persistence.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Entities = Hao.GroupBlog.Persistence.Entities;

namespace Hao.GroupBlog.Manager.Implements
{
    public class SeriesManager : BaseManager, ISeriesManager
    {
        private readonly ILogger _logger;
        private const string DOMAIN_GROUP_KEY = "domain";

        public SeriesManager(GbDbContext dbContext,
            IMapper mapper,
            ILogger<SeriesManager> logger,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
            : base(dbContext, mapper, configuration, httpContextAccessor)
        {
            _logger = logger;
        }

        public async Task<ResponseResult<DomainM>> AddDomain(DomainM model)
        {
            var res = new ResponseResult<DomainM>();
            try
            {
                var exist = await _dbContext.Domain.AnyAsync(x => x.Name == model.Name && x.CreatedById == CurrentUserId && !x.Deleted);
                if (exist) throw new Exception("存在同名领域！");
                var entity = _mapper.Map<DomainM, Entities.Domain>(model);
                entity.Id = entity.GetId(MachineCode);
                entity.CreatedById = CurrentUserId;
                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                res.Data = _mapper.Map<Entities.Domain, DomainM>(entity);
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
                if (string.IsNullOrEmpty(model.Id)) throw new Exception("领域标识为空！");
                var exist = await _dbContext.Domain.AnyAsync(x => x.Id != model.Id && x.Name == model.Name && x.CreatedById == CurrentUserId && !x.Deleted);
                if (exist) throw new Exception("存在同名领域！");
                var entity = await _dbContext.Domain.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (entity == null) throw new Exception("领域数据为空！");

                entity.Name = model.Name;
                entity.LastModifiedById = CurrentUserId;
                entity.LastModifiedAt = DateTime.Now;
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
                var entity = await _dbContext.Domain.FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null) throw new Exception("领域数据为空！");
                var exist = await _dbContext.Topic.AnyAsync(x => !x.Deleted && x.DomainId == entity.Id);
                if (exist) throw new Exception("该领域下存在主题，不能删除！");

                entity.DeletedById = CurrentUserId;
                entity.DeletedAt = DateTime.Now;
                entity.Deleted = true;
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
                var domains = await _dbContext.Domain.AsNoTracking().Where(x => !x.Deleted && x.CreatedById == CurrentUserId).ToListAsync();
                var ds = _mapper.Map<List<Entities.Domain>, List<DomainM>>(domains);
                var groupkey = $"{DOMAIN_GROUP_KEY}_{CurrentUserId}";
                var sequences = await _dbContext.Sequence.AsNoTracking().Where(x => x.GroupKey == groupkey).ToListAsync();

                var dsIds = ds.Select(x => x.Id).ToList();
                var topics = await _dbContext.Topic.AsNoTracking().Where(x => !x.Deleted && dsIds.Contains(x.DomainId)).ToListAsync();
                var ts = _mapper.Map<List<Entities.Topic>, List<TopicM>>(topics);
                var qc = await _dbContext.Sequence.AsNoTracking().Where(x => dsIds.Contains(x.GroupKey)).ToListAsync();

                ds.ForEach(x =>
                {
                    var index = sequences.FirstOrDefault(y => y.TrgetId == x.Id)?.Order;
                    if (index == null) index = 1024;
                    x.Order = index.Value;
                    var sts = ts.Where(y => y.DomainId == x.Id).ToList();
                    var qcs = qc.Where(y => y.GroupKey == x.Id).ToList();
                    x.Topics = this.SortTopics(sts, qcs);
                });
                res.Data = ds.OrderBy(x => x.Order).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"获取领域列表失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> SortDomain(List<OptionItem<int>> sequnces)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var groupkey = $"{DOMAIN_GROUP_KEY}_{CurrentUserId}";
                var olds = await _dbContext.Sequence.Where(x => x.GroupKey == groupkey).ToListAsync();
                _dbContext.RemoveRange(olds);

                var news = new List<Entities.Sequence>();
                sequnces.ForEach(x =>
                {
                    news.Add(new Entities.Sequence() { GroupKey = groupkey, TrgetId = x.Value, Order = x.Key });
                });
                await _dbContext.AddRangeAsync(news);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"领域排序失败！");
            }
            return res;
        }


        public async Task<ResponseResult<TopicM>> AddTopic(TopicM model)
        {
            var res = new ResponseResult<TopicM>();
            try
            {
                var exist = await _dbContext.Topic.AnyAsync(x => x.Name == model.Name && x.DomainId == model.DomainId && !x.Deleted);
                if (exist) throw new Exception("领域下存在同名主题！");
                var entity = _mapper.Map<TopicM, Entities.Topic>(model);
                entity.Id = entity.GetId(MachineCode);
                entity.CreatedById = CurrentUserId;
                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                res.Data = _mapper.Map<Entities.Topic, TopicM>(entity);
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
                if (string.IsNullOrEmpty(model.Id)) throw new Exception("主题标识为空！");
                var exist = await _dbContext.Topic.AnyAsync(x => x.Id != model.Id && x.Name == model.Name && x.DomainId == model.DomainId && !x.Deleted);
                if (exist) throw new Exception("存在同名主题！");
                var entity = await _dbContext.Topic.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (entity == null) throw new Exception("主题数据为空！");

                entity.Name = model.Name;
                //entity.Logo = model.Logo;
                entity.LastModifiedById = CurrentUserId;
                entity.LastModifiedAt = DateTime.Now;
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
                var entity = await _dbContext.Topic.FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null) throw new Exception("主题数据为空！");
                var exist = await _dbContext.Column.AnyAsync(x => !x.Deleted && x.TopicId == entity.Id);
                if (exist) throw new Exception("该主题下存在专栏，不能删除！");

                entity.DeletedById = CurrentUserId;
                entity.DeletedAt = DateTime.Now;
                entity.Deleted = true;
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
                var entities = await _dbContext.Topic.AsNoTracking().Where(x => !x.Deleted && x.CreatedById == CurrentUserId).ToListAsync();
                var data = _mapper.Map<List<Entities.Topic>, List<TopicM>>(entities);
                var sequences = await _dbContext.Sequence.AsNoTracking().Where(x => x.GroupKey == domainId).ToListAsync();

                data.ForEach(x =>
                {
                    var index = sequences.FirstOrDefault(y => y.TrgetId == x.Id)?.Order;
                    if (index == null) index = 1024;
                    x.Order = index.Value;
                });
                res.Data = data.OrderBy(x => x.Order).ToList();
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
                var entity = await _dbContext.Topic.FirstOrDefaultAsync(x => x.Id == model.DragObjectId);
                if (entity == null) throw new Exception("主题数据为空！");
                if (entity.DomainId != model.DropGroupId)
                {
                    entity.DomainId = model.DropGroupId;
                    entity.LastModifiedById = CurrentUserId;
                    entity.LastModifiedAt = DateTime.Now;
                }

                var olds = await _dbContext.Sequence.Where(x => x.GroupKey == entity.DomainId).ToListAsync();
                _dbContext.RemoveRange(olds);

                var news = new List<Entities.Sequence>();
                model.DropTargets.ForEach(x =>
                {
                    news.Add(new Entities.Sequence() { GroupKey = entity.DomainId, TrgetId = x.Value, Order = x.Key });
                });
                await _dbContext.AddRangeAsync(news);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"领域【{model.DropGroupId}】下的主题排序失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> AddTopicLogo(string id,string logo)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var entity = await _dbContext.Topic.FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null) throw new Exception("主题数据为空！");
                entity.Logo = logo;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"添加主题【{id}】Logo失败！");
            }
            return res;
        }


        public async Task<ResponseResult<ColumnM>> AddColumn(ColumnM model)
        {
            var res = new ResponseResult<ColumnM>();
            try
            {
                var exist = await _dbContext.Column.AnyAsync(x => x.Name == model.Name && x.TopicId == model.TopicId && !x.Deleted);
                if (exist) throw new Exception("主题下存在同名专栏！");
                var entity = _mapper.Map<ColumnM, Entities.Column>(model);
                entity.Id = entity.GetId(MachineCode);
                entity.CreatedById = CurrentUserId;
                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                res.Data = _mapper.Map<Entities.Column, ColumnM>(entity);
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
                if (string.IsNullOrEmpty(model.Id)) throw new Exception("专栏标识为空！");
                var exist = await _dbContext.Column.AnyAsync(x => x.Id != model.Id && x.Name == model.Name && x.TopicId == model.TopicId && !x.Deleted);
                if (exist) throw new Exception("存在同名专栏！");
                var entity = await _dbContext.Column.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (entity == null) throw new Exception("专栏数据为空！");

                entity.Name = model.Name;
                entity.Logo = model.Logo;
                entity.Intro = model.Intro;
                entity.LastModifiedById = CurrentUserId;
                entity.LastModifiedAt = DateTime.Now;
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
                var entity = await _dbContext.Column.FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null) throw new Exception("主题数据为空！");
                var exist = await _dbContext.Note.AnyAsync(x => !x.Deleted && x.ColumnId == entity.Id);
                if (exist) throw new Exception("该主题下存在笔记，不能删除！");

                entity.DeletedById = CurrentUserId;
                entity.DeletedAt = DateTime.Now;
                entity.Deleted = true;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"删除专栏【{id}】失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<ColumnM>> GetColumnList(string topicId)
        {
            var res = new ResponsePagingResult<ColumnM>();
            try
            {
                var entities = await _dbContext.Column.AsNoTracking().Where(x => !x.Deleted && x.CreatedById == CurrentUserId).ToListAsync();
                var data = _mapper.Map<List<Entities.Column>, List<ColumnM>>(entities);
                var sequences = await _dbContext.Sequence.AsNoTracking().Where(x => x.GroupKey == topicId).ToListAsync();

                data.ForEach(x =>
                {
                    var index = sequences.FirstOrDefault(y => y.TrgetId == x.Id)?.Order;
                    if (index == null) index = 1024;
                    x.Order = index.Value;
                });
                res.Data = data.OrderBy(x => x.Order).ToList();
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
                var entity = await _dbContext.Column.FirstOrDefaultAsync(x => x.Id == model.DragObjectId);
                if (entity == null) throw new Exception("专栏数据为空！");
                if (entity.TopicId != model.DropGroupId)
                {
                    entity.TopicId = model.DropGroupId;
                    entity.LastModifiedById = CurrentUserId;
                    entity.LastModifiedAt = DateTime.Now;
                }

                var olds = await _dbContext.Sequence.Where(x => x.GroupKey == model.DropGroupId).ToListAsync();
                _dbContext.RemoveRange(olds);

                var news = new List<Entities.Sequence>();
                model.DropTargets.ForEach(x =>
                {
                    news.Add(new Entities.Sequence() { GroupKey = entity.TopicId, TrgetId = x.Value, Order = x.Key });
                });
                await _dbContext.AddRangeAsync(news);
                await _dbContext.SaveChangesAsync();
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"主题【{model.DropGroupId}】下的专栏排序失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> AddColumnLogo(string id, string logo)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var entity = await _dbContext.Column.FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null) throw new Exception("专栏数据为空！");
                entity.Logo = logo;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"添加专栏【{id}】Logo失败！");
            }
            return res;
        }


        private List<TopicM> SortTopics(List<TopicM> topics, List<Entities.Sequence> sequences)
        {
            if (!topics.Any()) return topics;
            topics.ForEach(x =>
            {
                var index = sequences.FirstOrDefault(y => y.TrgetId == x.Id)?.Order;
                if (index == null) index = 1024;
                x.Order = index.Value;
            });
            return topics.OrderBy(x => x.Order).ToList();
        }
    }
}
