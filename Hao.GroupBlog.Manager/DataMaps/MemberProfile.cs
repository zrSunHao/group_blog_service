using AutoMapper;
using Hao.GroupBlog.Common.Enums;
using Hao.GroupBlog.Domain.Models;
using Hao.GroupBlog.Persistence.Entities;

namespace Hao.GroupBlog.Manager.DataMaps
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<LoginM, Member>()
               .ForMember(x => x.Id, y => y.Ignore())
               .ForMember(x => x.Password, y => y.Ignore())
               .ForMember(x => x.PasswordSalt, y => y.Ignore())
               .ForMember(x => x.UserName, y => y.MapFrom(z => z.UserName))
               .ForMember(x => x.Role, y => y.MapFrom(z => RoleType.ordinary))
               .ForMember(x => x.Remark, y => y.MapFrom(z => "无"))
               .ForMember(x => x.Limited, y => y.MapFrom(z => false))
               .ForMember(x => x.LastModifiedAt, y => y.MapFrom(z => DateTime.Now))
               .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.Now))
               .ForMember(x => x.Deleted, y => y.MapFrom(z => false));

            CreateMap<Member, UserLastLoginRecord>()
                 .ForMember(x => x.Id, y => y.Ignore())
                 .ForMember(x => x.LoginId, y => y.MapFrom(z => Guid.NewGuid()))
                 .ForMember(x => x.MemberId, y => y.MapFrom(z => z.Id))
                 .ForMember(x => x.Role, y => y.MapFrom(z => z.Role))
                 .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.Now))
                 .ForMember(x => x.ExpiredAt, y => y.MapFrom(z => DateTime.Now.AddDays(1)));
        }
    }
}
