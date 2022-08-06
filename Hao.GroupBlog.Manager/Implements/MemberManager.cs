using AutoMapper;
using Hao.GroupBlog.Domain.Consts;
using Hao.GroupBlog.Domain.Interfaces;
using Hao.GroupBlog.Domain.Models;
using Hao.GroupBlog.Domain.Paging;
using Hao.GroupBlog.Manager.Basic;
using Hao.GroupBlog.Manager.Providers;
using Hao.GroupBlog.Persistence.Database;
using Hao.GroupBlog.Persistence.Entities;
using Hao.GroupBlog.Utils.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hao.GroupBlog.Manager.Implements
{
    public class MemberManager : BaseManager, IMemberManager
    {
        private readonly ILogger _logger;
        private readonly ICacheProvider _cache;

        public MemberManager(GbDbContext dbContext,
            IMapper mapper,
            ICacheProvider cache,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<MemberManager> logger)
            : base(dbContext, mapper, configuration, httpContextAccessor)
        {
            _logger = logger;
            _cache = cache;
        }

        private static object _lock = new object();
        public async Task<ResponseResult<bool>> Register(LoginM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                lock (_lock)
                {
                    var exist = _dbContext.Member.Any(x => x.UserName == model.UserName);
                    if (exist) throw new MyCustomException("账号已存在！");
                }

                Member member = _mapper.Map<LoginM, Member>(model);
                member.Id = member.GetId(MachineCode);
                member.CreatedById = member.Id;
                HashHandler.CreateHash(model.Password, out var hash, out var salt);
                member.Password = hash;
                member.PasswordSalt = salt;

                await _dbContext.AddAsync(member);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"账号【{model.UserName}】注册失败！");
            }
            return res;
        }

        public async Task<ResponseResult<LoginRes>> Login(LoginM model)
        {
            var res = new ResponseResult<LoginRes>();
            try
            {
                Member? member = await _dbContext.Member.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserName == model.UserName && !x.Deleted);
                if (member == null) throw new MyCustomException("账号或密码不正确！");
                var valid = HashHandler.VerifyHash(model.Password, member.Password, member.PasswordSalt);
                if (!valid) throw new MyCustomException("账号或密码不正确！");
                if(member.Limited) throw new MyCustomException("账号被锁定，请联系管理员！");

                var record = await _dbContext.UserLastLoginRecord.FirstOrDefaultAsync(x => x.MemberId == member.Id);
                if(record == null)
                {
                    record = _mapper.Map<Member, UserLastLoginRecord>(member);
                    await _dbContext.AddAsync(record);
                }
                else
                {
                    record.LoginId = Guid.NewGuid();
                    record.Role = member.Role;
                    record.CreatedAt = DateTime.Now;
                    record.ExpiredAt = DateTime.Now.AddDays(1);
                }
                await _dbContext.SaveChangesAsync();

                _cache.Save(record.LoginId.ToString(), record);
                var token = this.BuilderToken(record.LoginId.ToString(), member.UserName, record.ExpiredAt);
                res.Data = new LoginRes()
                {
                    UserName = member.UserName,
                    Role = member.Role,
                    Key = record.LoginId.ToString(),
                    Token = token
                };
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"账号【{model.UserName}】登录失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Logout()
        {
            var res = new ResponseResult<bool>();
            try
            {
                var loginId = this.CurrentLoginId;
                var entity = await _dbContext.UserLastLoginRecord.FirstOrDefaultAsync(x => x.LoginId == loginId);
                if (entity != null)
                {
                    entity.ExpiredAt = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                }
                _cache.Remove(loginId.ToString());
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"用户【{CurrentLoginId}】登出失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Update(MemberM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                Member? member = await _dbContext.Member
                    .FirstOrDefaultAsync(x => x.UserName == model.UserName && !x.Deleted);
                if (member == null) throw new MyCustomException("未查询到用户数据！");
                member.Remark = model.Remark;
                member.Role = model.Role;
                member.Limited = model.Limited;
                member.LastModifiedAt = DateTime.Now;
                member.LastModifiedById = CurrentUserId;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"更新账号【{model.UserName}】户信息失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> ManagerResetPsd(ResetPsdM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var myRole = CurrentUserRole;
                var entity = await _dbContext.Member
                    .FirstOrDefaultAsync(x => x.UserName == model.UserName);
                if (entity == null) throw new MyCustomException("未查询到账号数据！");
                if(entity.Role > myRole) throw new MyCustomException("您的角色权限低于该账号！");

                HashHandler.CreateHash(model.NewPsd, out var hash, out var salt);
                entity.Password = hash;
                entity.PasswordSalt = salt;
                entity.LastModifiedAt = DateTime.Now;
                entity.LastModifiedById = CurrentUserId;

                var record = await _dbContext.UserLastLoginRecord.FirstOrDefaultAsync(x => x.MemberId == entity.Id);
                if (record != null)
                {
                    record.ExpiredAt = DateTime.Now;
                    _cache.Remove(record.LoginId.ToString());
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"管理员重置账号为【{model.UserName}】的密码失败！");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> MyselfResetPsd(ResetPsdM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                if(string.IsNullOrEmpty(model.OldPsd)) throw new MyCustomException("原密码为空！");
                var entity = await _dbContext.Member
                    .FirstOrDefaultAsync(x => x.Id == CurrentUserId);
                if (entity == null) throw new MyCustomException("未查询到账号数据！");
                var valid = HashHandler.VerifyHash(model.OldPsd, entity.Password, entity.PasswordSalt);
                if (!valid) throw new MyCustomException("原密码不正确！");

                HashHandler.CreateHash(model.NewPsd, out var hash, out var salt);
                entity.Password = hash;
                entity.PasswordSalt = salt;
                entity.LastModifiedAt = DateTime.Now;
                entity.LastModifiedById = CurrentUserId;

                var record = await _dbContext.UserLastLoginRecord.FirstOrDefaultAsync(x => x.MemberId == entity.Id);
                if (record != null)
                {
                    record.ExpiredAt = DateTime.Now;
                    _cache.Remove(record.LoginId.ToString());
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"用户【{model.UserName}】重置密码失败！");
            }
            return res;
        }

        public async Task<ResponsePagingResult<MemberM>> GetList(PagingParameter<MemberFilter> parameter)
        {
            var res = new ResponsePagingResult<MemberM>();
            try
            {
                var myRole = CurrentUserRole;
                var query = from m in _dbContext.Member
                            join r in _dbContext.UserLastLoginRecord on m.Id equals r.MemberId into z
                            from t in z.DefaultIfEmpty()
                            where !m.Deleted && m.Role <= myRole
                            select new MemberM()
                            {
                                Id = m.Id,
                                UserName = m.UserName,
                                Role = m.Role,
                                Remark = m.Remark,
                                Limited = m.Limited,
                                LastLoginAt = t.CreatedAt
                            };
                var filter = parameter.Filter;
                if (filter != null)
                {
                    if (!string.IsNullOrEmpty(filter.UserName)) query = query.Where(x => x.UserName.Contains(filter.UserName));
                    if (filter.Role.HasValue && filter.Role != 0) query = query.Where(x => x.Role == filter.Role.Value);
                    if (filter.Limited.HasValue) query = query.Where(x => x.Limited == filter.Limited.Value);
                    if (filter.StartAt.HasValue) query = query.Where(x => x.LastLoginAt >= filter.StartAt.Value);
                    if (filter.EndAt.HasValue) query = query.Where(x => x.LastLoginAt <= filter.EndAt.Value.AddDays(1).AddSeconds(-1));
                }

                query = query.OrderByDescending(x => x.LastLoginAt);
                if (parameter.Sort != null && parameter.Sort.ToLower() == "desc")
                {
                    if (parameter.SortColumn?.ToLower() == "UserName".ToLower())
                        query = query.OrderByDescending(x => x.UserName);
                }
                else
                {
                    if (parameter.SortColumn?.ToLower() == "UserName".ToLower())
                        query = query.OrderBy(x => x.UserName);
                    if (parameter.SortColumn?.ToLower() == "LastLoginAt".ToLower())
                        query = query.OrderBy(x => x.LastLoginAt);
                }

                res.RowsCount = await query.CountAsync();
                query = query.AsPaging(parameter.PageIndex, parameter.PageSize);
                res.Data = await query.ToListAsync();
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取用户列表失败！");
            }
            return res;
        }


        private string BuilderToken(string recordId, string userName, DateTime expiredAt)
        {
            var key = GetConfiguration(CfgConsts.PLATFORM_KEY);
            var issuer = GetConfiguration(CfgConsts.PLATFORM_ISSUER);
            var msg = new TokenMsg
            {
                Id = recordId,
                Name = userName,
                ExpiredAt = expiredAt,
                Key = key,
                Issuer = issuer,
            };
            var handler = new TokenHandler();
            return handler.BuilderToken(msg);
        }
    }
}
