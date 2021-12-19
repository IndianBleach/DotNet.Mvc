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
        IEnumerable<HomeIdeaDto> GetIdeasPerPage(int page);
        Guid CreateIdea(CreateIdeaDto item);
        IEnumerable<HomeIdeaDto> GetIdeasWithQuery(string query, int page);
        int GetCount();
        void Save();
    }
}
