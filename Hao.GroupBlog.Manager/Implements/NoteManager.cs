using AutoMapper;
using Hao.GroupBlog.Domain.Interfaces;
using Hao.GroupBlog.Domain.Models;
using Hao.GroupBlog.Domain.Paging;
using Hao.GroupBlog.Manager.Basic;
using Hao.GroupBlog.Persistence.Database;
using Hao.GroupBlog.Persistence.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hao.GroupBlog.Manager.Implements
{
    public class NoteManager : BaseManager, INoteManager
    {
        private readonly ILogger _logger;

        public NoteManager(GbDbContext dbContext,
            IMapper mapper,
            ILogger<NoteManager> logger,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
            : base(dbContext, mapper, configuration, httpContextAccessor)
        {
            _logger = logger;
        }

        public async Task<bool> IsOpen(string noteId, string fileName)
        {
            var res = false;
            try
            {
                var entity = await (from r in _dbContext.FileResource
                                    join n in _dbContext.Note on r.OwnId equals n.ContentId
                                    where r.FileName == fileName
                                    where !n.Deleted && n.Opened && n.ContentId == noteId
                                    select n)
                .FirstOrDefaultAsync();
                if (entity == null) return false;
                return entity.Opened;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"获取笔记[{noteId}]文件[{fileName}]分享状态失败！");
            }
            return res;
        }

        public async Task<ResponseResult<NoteM>> Add(NoteM model)
        {
            var res = new ResponseResult<NoteM>();
            try
            {
                var content = new NoteContent();
                content.Id = content.GetId(MachineCode);
                content.Content = "[{\"type\":2,\"content\":\"新标题\",\"url\":\"\",\"remark\":\"\",\"order\":1,\"data\":{},\"open\":false,\"children\":[]}]";
                content.CreatedAt = DateTime.Now;
                content.CreatedById = CurrentUserId;

                Note note = _mapper.Map<NoteM, Note>(model);
                note.ContentId = content.Id;
                note.CreatedById = CurrentUserId;

                await _dbContext.AddAsync(note);
                await _dbContext.AddAsync(content);
                await _dbContext.SaveChangesAsync();
                var m = _mapper.Map<Note, NoteM>(note);
                m.Author = CurrentUserName;
                res.Data = m;
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"添加笔记【{model.Name}】信息失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Update(NoteM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                if (string.IsNullOrEmpty(model.ContentId)) throw new Exception("笔记标识为空！");
                var entity = await _dbContext.Note.FirstOrDefaultAsync(x => x.ContentId == model.ContentId);
                if (entity == null) throw new Exception("笔记数据为空！");

                entity.Name = model.Name;
                entity.Keys = model.Keys;
                entity.ProfileName = model.ProfileName;
                entity.Intro = model.Intro;
                entity.LastModifiedById = CurrentUserId;
                entity.LastModifiedAt = DateTime.Now;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"更新笔记【{model.Name}】信息失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Open(string id, bool opened)
        {
            var res = new ResponseResult<bool>();
            try
            {
                if (string.IsNullOrEmpty(id)) throw new Exception("笔记标识为空！");
                var entity = await _dbContext.Note.FirstOrDefaultAsync(x => x.ContentId == id);
                if (entity == null) throw new Exception("笔记数据为空！");
                if (entity.CreatedById != CurrentUserId) throw new Exception("不是笔记的所有者，无权操作！");

                entity.Opened = opened;
                entity.LastModifiedById = CurrentUserId;
                entity.LastModifiedAt = DateTime.Now;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"更新笔记【{id}】分享状态失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> ToColumn(string id, string columnId)
        {
            var res = new ResponseResult<bool>();
            try
            {
                if (string.IsNullOrEmpty(id)) throw new Exception("笔记标识为空！");
                var entity = await _dbContext.Note.FirstOrDefaultAsync(x => x.ContentId == id);
                if (entity == null) throw new Exception("笔记数据为空！");
                entity.ColumnId = columnId;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"移动笔记【{id}】到专栏【{columnId}】失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> AddProfile(string id, string profileName)
        {
            var res = new ResponseResult<bool>();
            try
            {
                if (string.IsNullOrEmpty(id)) throw new Exception("笔记标识为空！");
                var entity = await _dbContext.Note.FirstOrDefaultAsync(x => x.ContentId == id);
                if (entity == null) throw new Exception("笔记数据为空！");
                entity.ProfileName = profileName;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"添加笔记【{id}】封面失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Hit(string id)
        {
            var res = new ResponseResult<bool>();
            try
            {
                if (string.IsNullOrEmpty(id)) throw new Exception("笔记标识为空！");
                var entity = await _dbContext.Note.FirstOrDefaultAsync(x => x.ContentId == id);
                if (entity == null) throw new Exception("笔记数据为空！");
                entity.Hits++;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"修改笔记【{id}】点击次数失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Delete(string id)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var note = await _dbContext.Note.FirstOrDefaultAsync(x => x.ContentId == id);
                if (note == null) throw new Exception("笔记数据为空！");
                note.DeletedById = CurrentUserId;
                note.DeletedAt = DateTime.Now;
                note.Deleted = true;

                var content = await _dbContext.NoteContent.FirstOrDefaultAsync(x => x.Id == id);
                if (content == null) throw new Exception("内容数据为空！");
                content.DeletedById = CurrentUserId;
                content.DeletedAt = DateTime.Now;
                content.Deleted = true;

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"删除笔记【{id}】失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<NoteM>> GetMyList(string columnId)
        {
            var res = new ResponsePagingResult<NoteM>();
            try
            {
                var entities = await _dbContext.Note.AsNoTracking()
                    .Where(x => !x.Deleted && x.CreatedById == CurrentUserId && x.ColumnId == columnId)
                    .OrderByDescending(x => x.LastModifiedAt)
                    .ToListAsync();
                res.RowsCount = entities.Count();
                var data = _mapper.Map<List<Note>, List<NoteM>>(entities);
                var sequences = await _dbContext.Sequence.AsNoTracking().Where(x => x.GroupKey == columnId).ToListAsync();
                data.ForEach(x =>
                {
                    var index = sequences.FirstOrDefault(y => y.TrgetId == x.ContentId)?.Order;
                    if (index == null) index = 1024;
                    x.Order = index.Value;
                    x.Author = CurrentUserName;
                });
                res.Data = data.OrderBy(x => x.Order).ToList();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取专栏【{columnId}】下我的笔记列表失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> SortMyNotes(SequnceM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var olds = await _dbContext.Sequence.Where(x => x.GroupKey == model.DropGroupId).ToListAsync();
                _dbContext.RemoveRange(olds);

                var news = new List<Sequence>();
                model.DropTargets.ForEach(x =>
                {
                    news.Add(new Sequence() { GroupKey = model.DropGroupId, TrgetId = x.Value, Order = x.Key });
                });
                await _dbContext.AddRangeAsync(news);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"专栏【{model.DropGroupId}】下的笔记排序失败！");
                res.AddError(e);
            }
            return res;
        }



        public async Task<ResponseResult<bool>> Favorite(string id, string? columnId)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var entity = await _dbContext.Favorite.FirstOrDefaultAsync(x => x.NoteId == id && x.MemberId == CurrentUserId);
                if (entity == null)
                {
                    entity = new Favorite()
                    {
                        MemberId = CurrentUserId,
                        NoteId = id,
                        ColumnId = columnId,
                        CreatedAt = DateTime.Now,
                    };
                    await _dbContext.AddAsync(entity);
                }
                else
                {
                    entity.ColumnId = columnId;
                    entity.CreatedAt = DateTime.Now;
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"收藏笔记【{id}】到专栏【{columnId}】失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> CancelFavorite(string id)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var entity = await _dbContext.Favorite.FirstOrDefaultAsync(x => x.NoteId == id && x.MemberId == CurrentUserId);
                if (entity == null) return res;
                _dbContext.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"取消收藏笔记【{id}】失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<NoteM>> GetFavoriteList(string columnId)
        {
            var res = new ResponsePagingResult<NoteM>();
            try
            {
                var query = from f in _dbContext.Favorite
                            join n in _dbContext.Note on f.NoteId equals n.ContentId
                            where f.MemberId == CurrentUserId && f.ColumnId == columnId
                            where !n.Deleted && n.Opened
                            select n;
                res.RowsCount = await query.CountAsync();
                var entities = await query.ToListAsync();

                var data = _mapper.Map<List<Note>, List<NoteM>>(entities);
                var userIds = data.Select(x => x.CreatedById).ToList();
                var dcs = await _dbContext.Member.AsNoTracking()
                    .Where(x => userIds.Contains(x.Id))
                    .Select(y => new { Id = y.Id, UserName = y.UserName })
                    .ToListAsync();
                var sequences = await _dbContext.Sequence.AsNoTracking().Where(x => x.GroupKey == columnId).ToListAsync();
                data.ForEach(x =>
                {
                    x.ColumnId = columnId;
                    var user = dcs.FirstOrDefault(y => y.Id == x.CreatedById);
                    if (user != null) x.Author = user.UserName;
                    var index = sequences.FirstOrDefault(y => y.TrgetId == x.ContentId)?.Order;
                    if (index == null) index = 1024;
                    x.Order = index.Value;
                });
                res.Data = data.OrderBy(x => x.Order).ToList();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取笔记列表失败！搜索条件为【{columnId}】");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> SortFavoriteNotes(SequnceM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var olds = await _dbContext.Sequence.Where(x => x.GroupKey == model.DropGroupId).ToListAsync();
                _dbContext.RemoveRange(olds);

                var news = new List<Sequence>();
                model.DropTargets.ForEach(x =>
                {
                    news.Add(new Sequence() { GroupKey = model.DropGroupId, TrgetId = x.Value, Order = x.Key });
                });
                await _dbContext.AddRangeAsync(news);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"收藏专栏【{model.DropGroupId}】下的笔记排序失败！");
                res.AddError(e);
            }
            return res;
        }



        public async Task<ResponseResult<NoteContentM>> GetContent(string id)
        {
            var res = new ResponseResult<NoteContentM>();
            try
            {
                var entity = await _dbContext.NoteContent.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null) throw new Exception("笔记内容数据未查询到！");
                var content = new NoteContentM()
                {
                    Id = id,
                    Content = entity.Content
                };
                res.Data = content;
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取笔记【{id}】内容失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> SaveContent(NoteContentM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var note = await _dbContext.Note.FirstOrDefaultAsync(x => x.ContentId == model.Id);
                if (note == null) throw new Exception("笔记内容数据未查询到！");
                note.LastModifiedById = CurrentUserId;
                note.LastModifiedAt = DateTime.Now;

                var entity = await _dbContext.NoteContent.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (entity == null) throw new Exception("笔记内容数据未查询到！");

                entity.Backups3 = entity.Backups2;
                entity.Backups2 = entity.Backups1;
                entity.Backups1 = entity.Content;
                entity.Content = model.Content;
                entity.LastModifiedAt = DateTime.Now;
                entity.LastModifiedById = CurrentUserId;

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"保存笔记【{model.Id}】的内容失败！");
            }
            return res;
        }



        public async Task<ResponseResult<NoteContentM>> GetOpenedContent(string id)
        {
            var res = new ResponseResult<NoteContentM>();
            try
            {
                var entity = await (from c in _dbContext.NoteContent
                                    join n in _dbContext.Note on c.Id equals n.ContentId
                                    where c.Id == id && !c.Deleted
                                    where !n.Deleted && n.Opened
                                    select c).FirstOrDefaultAsync();
                if (entity == null) throw new Exception("笔记内容数据未开放或未查询到！");
                var content = new NoteContentM()
                {
                    Id = id,
                    Content = entity.Content
                };
                res.Data = content;
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取分享笔记【{id}】内容失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<NoteM>> GetOpenedList(PagingParameter<string> parameter)
        {
            var res = new ResponsePagingResult<NoteM>();
            try
            {
                var query = _dbContext.Note.AsNoTracking().Where(x => !x.Deleted && x.Opened);
                if (!string.IsNullOrEmpty(parameter.Filter))
                    query = query.Where(x => x.Name.Contains(parameter.Filter) || x.Keys.Contains(parameter.Filter));

                query = query.OrderByDescending(x => x.LastModifiedAt);
                if (parameter.Sort != null && parameter.Sort.ToLower() == "desc")
                {
                    if (parameter.SortColumn?.ToLower() == "Name".ToLower())
                        query = query.OrderByDescending(x => x.Name);
                }
                else
                {
                    if (parameter.SortColumn?.ToLower() == "Name".ToLower())
                        query = query.OrderBy(x => x.Name);
                    if (parameter.SortColumn?.ToLower() == "LastModifiedAt".ToLower())
                        query = query.OrderBy(x => x.LastModifiedAt);
                }
                res.RowsCount = await query.CountAsync();
                query = query.AsPaging(parameter.PageIndex, parameter.PageSize);
                var entities = await query.ToListAsync();

                var data = _mapper.Map<List<Note>, List<NoteM>>(entities);
                var userIds = data.Select(x => x.CreatedById).ToList();
                var us = await _dbContext.Member.AsNoTracking()
                    .Where(x => userIds.Contains(x.Id))
                    .Select(y => new { Id = y.Id, UserName = y.UserName })
                    .ToListAsync();
                var noteIds = data.Select(x => x.ContentId).ToList();
                var ns = await _dbContext.Favorite.AsNoTracking()
                    .Where(x => noteIds.Contains(x.NoteId) && x.MemberId == CurrentUserId)
                    .Select(y => y.NoteId)
                    .ToListAsync();
                data.ForEach(x =>
                {
                    var user = us.FirstOrDefault(y => y.Id == x.CreatedById);
                    if (user != null) x.Author = user.UserName;
                    x.Checked = ns.Any(y => y == x.ContentId);
                });
                res.Data = data;
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取公开笔记列表失败！搜索条件为【{parameter.Filter}】");
            }
            return res;
        }
    }
}
