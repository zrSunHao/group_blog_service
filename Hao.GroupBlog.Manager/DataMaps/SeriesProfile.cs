using AutoMapper;
using Hao.GroupBlog.Domain.Models;
using Entities = Hao.GroupBlog.Persistence.Entities;

namespace Hao.GroupBlog.Manager.DataMaps
{
    public class SeriesProfile : Profile
    {
        public SeriesProfile()
        {
            CreateMap<DomainM, Entities.Domain>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.Now));
            CreateMap<Entities.Domain, DomainM>()
                .ForMember(x => x.Order, y => y.Ignore());

            CreateMap<TopicM, Entities.Topic>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.Now));
            CreateMap<Entities.Topic, TopicM>()
                .ForMember(x => x.Order, y => y.Ignore());

            CreateMap<ColumnM, Entities.Column>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.Now));
            CreateMap<Entities.Column, ColumnM>()
                .ForMember(x => x.Order, y => y.Ignore());
        }
    }
}
