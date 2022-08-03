using AutoMapper;
using Hao.GroupBlog.Domain.Models;
using Hao.GroupBlog.Persistence.Entities;

namespace Hao.GroupBlog.Manager.DataMaps
{
    public class ResourceProfile : Profile
    {
        public ResourceProfile()
        {
            CreateMap<FileResource, ResourceM>();
            CreateMap<ResourceM, FileResource>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.Now));
        }
    }
}
