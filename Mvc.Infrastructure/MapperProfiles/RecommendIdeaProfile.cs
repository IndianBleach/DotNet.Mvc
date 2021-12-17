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
    public class RecommendIdeaProfile : Profile
    {
        public RecommendIdeaProfile()
        {
            CreateMap<Idea, IdeaRecommendationDto>()
                .ForMember("IdeaGuid", opt => opt.MapFrom(x => x.Guid))
                .ForMember("IdeaTitle", opt => opt.MapFrom(x => x.Title))
                .ForMember("AuthorName", opt => opt.MapFrom(x => x.Members
                    .FirstOrDefault(x => x.Role.Equals(IdeaMemberRoles.Author)).User.UserName))
                .ForMember("UserAvatarImageName", opt => opt.MapFrom(x => x.Members
                    .FirstOrDefault(x => x.Role.Equals(IdeaMemberRoles.Author)).User.Avatar.ImageName));
        }
    }
}
