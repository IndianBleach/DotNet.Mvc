using AutoMapper;
using Mvc.ApplicationCore.DTOs.Idea;
using Mvc.ApplicationCore.Entities.IdeaEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.Infrastructure.MapperProfiles
{
    public class SideIdeaProfile : Profile
    {
        public SideIdeaProfile()
        {
            CreateMap<Idea, SideIdeaDto>()
                .ForMember("Guid", opt => opt.MapFrom(x => x.Guid))
                .ForMember("Title", opt => opt.MapFrom(x => x.Title))
                .ForMember("AvatarImageName", opt => opt.MapFrom(x => x.Avatar.ImageName));
        }
    }
}
