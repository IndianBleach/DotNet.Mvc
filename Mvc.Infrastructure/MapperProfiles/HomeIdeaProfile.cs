using AutoMapper;
using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.DTOs.Idea;
using Mvc.ApplicationCore.Entities.IdeaEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.Infrastructure.MapperProfiles
{
    public class HomeIdeaProfile : Profile
    {
        public HomeIdeaProfile()
        {
            CreateMap<Idea, HomeIdeaDto>()
                .ForMember("Guid", opt => opt.MapFrom(x => x.Guid))
                .ForMember("Title", opt => opt.MapFrom(x => x.Title))
                .ForMember("Description", opt => opt.MapFrom(x => x.Topics
                    .FirstOrDefault(y => y.IsDefault == true).Description))
                .ForMember("TotalCommentsCount", opt => opt.MapFrom(x => x.Topics
                    .First().Comments.Count))
                .ForMember("TotalBoxCount", opt => opt.MapFrom(x => x.Boxes.Count))
                .ForMember("Tags", opt => opt.MapFrom(x => x.Tags
                    .Select(y => new TagDto(y.Name))));

        }
    }
}
