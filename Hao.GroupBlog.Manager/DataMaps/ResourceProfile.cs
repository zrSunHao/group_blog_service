using AutoMapper;
using Hao.GroupBlog.Domain.Models;
using Hao.GroupBlog.Persistence.Entities;

namespace Hao.GroupBlog.Manager.DataMaps
{
    public class ResourceProfile : Profile
    {
        public ResourceProfile()
        {
            CreateMap<FileResource, FileM>();
            CreateMap<FileM, FileResource>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.Now));

            CreateMap<FileResource, ResourceM>();
        }
    }
}
