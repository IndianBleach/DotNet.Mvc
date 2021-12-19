using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.DTOs.Idea;
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
