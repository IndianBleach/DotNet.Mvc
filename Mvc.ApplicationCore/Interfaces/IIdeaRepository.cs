using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.DTOs.Idea;
using Mvc.ApplicationCore.DTOs.Load;
using Mvc.ApplicationCore.Entities;
using Mvc.ApplicationCore.Entities.IdeaEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Interfaces
{
    public interface IIdeaRepository
    {
        Task<string> CickMember(string roleGuid);
        Task<bool> AddModder(string roleGuid);
        Task<bool> RemoveModder(string roleGuid);
        //
        Task<List<IdeaRoleDto>> GetIdeaRolesAsync(string ideaGuid);
        Task<bool> CreateTopic(string title, string description, string authorGuid, string ideaGuid);
        //
        IdeaMemberRoleDto GetIdeaMemberRole(string ideaGuid, string userGuid);
        List<IdeaTopicDto> GetIdeaTopics(string ideaGuid);
        IdeaDetailDto GetIdeaDetail(string ideaGuid);
        //
        IEnumerable<IdeaRecommendationDto> GetRecommendIdeas(string forUsername);
        IEnumerable<SideIdeaDto> GetSideIdeasByStatusFilter(IdeaStatuses filterStatus);        
        Guid CreateIdea(CreateIdeaDto item);
        List<HomeIdeaDto> GetIdeas(int page);
        List<HomeIdeaDto> GetIdeas(string query, int page);
        int GetCount();
        int GetCount(string query);
        void Save();
    }
}
