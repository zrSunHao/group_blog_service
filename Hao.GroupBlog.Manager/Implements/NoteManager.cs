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

        public async Task<bool> IsOpen(string id)
        {
            var res = false;
            try
            {
                var entity = await _dbContext.Note.AsNoTracking().FirstOrDefaultAsync(x => x.ContentId == id);
                if (entity == null) return false;
                return entity.Opened;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"获取笔记【{id}】分享状态失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Add(NoteM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var content = new NoteContent();
                content.Id = content.GetId(MachineCode);
                content.Content = "";
                content.CreatedAt = DateTime.Now;
                content.CreatedById = CurrentUserId;

                Note note = _mapper.Map<NoteM, Note>(model);
                note.ContentId = content.Id;

                await _dbContext.AddAsync(note);
                await _dbContext.AddAsync(content);
                await _dbContext.SaveChangesAsync();
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
                if(string.IsNullOrEmpty(model.ContentId)) throw new Exception("笔记标识为空！");
                var entity = await _dbContext.Note.FirstOrDefaultAsync(x => x.ContentId == model.ContentId);
                if (entity == null) throw new Exception("笔记数据为空！");

                entity.Name = model.Name;
                entity.Keys = model.Keys;
                entity.Opened = model.Opened;
                entity.ProfileName = model.ProfileName;
                entity.Intro = model.Intro;
                entity.LastModifiedById = CurrentUserId;
                entity.LastModifiedAt = DateTime.Now;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"添加笔记【{model.Name}】信息失败！");
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

        public async Task<ResponsePagingResult<NoteM>> GetMyList(PagingParameter<string> parameter)
        {
            var res = new ResponsePagingResult<NoteM>();
            try
            {
                var query = _dbContext.Note.AsNoTracking().Where(x => !x.Deleted && x.CreatedById == CurrentUserId);
                query = query.OrderByDescending(x => x.LastModifiedAt);
                if (string.IsNullOrEmpty(parameter.Filter))
                {
                    query = query.Where(x => x.ColumnId == parameter.Filter);
                    res.RowsCount = await query.CountAsync();
                }
                else
                {
                    res.RowsCount = await query.CountAsync();
                    query = query.AsPaging(parameter.PageIndex, parameter.PageSize);
                }
                var data = await query.ToListAsync();
                res.Data = _mapper.Map<List<Note>, List<NoteM>>(data);
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取笔记列表失败！搜索条件为【{parameter.Filter}】");
            }
            return res;
        }

        

        public async Task<ResponseResult<bool>> Favorite(string id, string? columnId)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var entity = await _dbContext.Favorite.FirstOrDefaultAsync(x=>x.NoteId == id && x.MemberId == CurrentUserId);
                if(entity == null)
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

        public async Task<ResponseResult<NoteContentM>> GetContent(string id)
        {
            var res = new ResponseResult<NoteContentM>();
            try
            {
                var entity = await _dbContext.NoteContent.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null) throw new Exception("笔记内容数据未查询到！");
                var content  = new NoteContentM()
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

        public async Task<ResponsePagingResult<NoteM>> GetFavoriteList(PagingParameter<string> parameter)
        {
            var res = new ResponsePagingResult<NoteM>();
            try
            {
                var query = from f in _dbContext.Favorite
                            join n in _dbContext.Note on f.NoteId equals n.ContentId
                            where f.MemberId == CurrentUserId
                            where !n.Deleted && n.Opened
                            select n;
                query = query.OrderByDescending(x => x.LastModifiedAt);
                if (string.IsNullOrEmpty(parameter.Filter))
                {
                    query = query.Where(x => x.ColumnId == parameter.Filter);
                    res.RowsCount = await query.CountAsync();
                }
                else
                {
                    res.RowsCount = await query.CountAsync();
                    query = query.AsPaging(parameter.PageIndex, parameter.PageSize);
                }
                var data = await query.ToListAsync();
                res.Data = _mapper.Map<List<Note>, List<NoteM>>(data);
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取笔记列表失败！搜索条件为【{parameter.Filter}】");
            }
            return res;
        }

        public async Task<ResponsePagingResult<NoteM>> GetOpenedList(PagingParameter<string> parameter)
        {
            var res = new ResponsePagingResult<NoteM>();
            try
            {
                var query = _dbContext.Note.AsNoTracking().Where(x => !x.Deleted && x.Opened);
                if (string.IsNullOrEmpty(parameter.Filter))
                    query = query.Where(x => x.Name.Contains(parameter.Filter) || x.Keys.Contains(parameter.Filter) || x.Intro.Contains(parameter.Filter));

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
                var data = await query.ToListAsync();
                res.Data = _mapper.Map<List<Note>, List<NoteM>>(data);
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取笔记列表失败！搜索条件为【{parameter.Filter}】");
            }
            return res;
        }
    }
}
