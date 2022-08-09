using AutoMapper;
using Hao.GroupBlog.Domain.Models;
using Hao.GroupBlog.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hao.GroupBlog.Manager.DataMaps
{
    public class NoteProfile : Profile
    {
        public NoteProfile()
        {
            CreateMap<NoteM, Note>()
               .ForMember(x => x.Id, y => y.Ignore())
               .ForMember(x => x.Hits, y => y.MapFrom(z => 0))
               .ForMember(x => x.Opened, y => y.MapFrom(z => false))
               .ForMember(x => x.LastModifiedAt, y => y.MapFrom(z => DateTime.Now))
               .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.Now));

            CreateMap<Note, NoteM>()
                .ForMember(x => x.Order, y => y.Ignore())
                .ForMember(x => x.Author, y => y.Ignore());
        }
    }
}
