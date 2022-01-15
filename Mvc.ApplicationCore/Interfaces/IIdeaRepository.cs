using Microsoft.AspNetCore.Http;
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
        List<SideIdeaDto> GetSimilarIdeas(string ideaGuid);
        //
        Task<bool> DeclineJoinAsync(string joinGuid);
        Task<bool> AcceptJoinAsync(string joinGuid);
        Task<List<JoinRequestDto>> GetIdeaJoinRequests(string ideaGuid, int page);
        //
        Task<string> UpdateIdeaSettingsAsync(IFormFile avatar, IdeaStatuses status, string description, bool isSecurity,string ideaGuid, string currentUserGuid);
        Task<bool> RemoveIdeaAsync(string ideaGuid, string confirmPassword, string currentUserGuid);
        //
        Task<bool> RemoveGoalAsync(string goalGuid, string currentUserGuid);
        Task<BoxGoalDetailDto> UpdateGoalStatusAsync(string goalGuid, BoxGoalStatuses newStatus, string currentUserGuid);
        //
        Task<BoxGoalDetailDto> CreateBoxGoalAsync(string boxGuid, string authorGuid, string content);
        //
        Task<List<IdeaMemberDto>> GetIdeaMembersAsync(string ideaGuid, int page);
        //
        Task<BoxDetailDto> GetBoxDetailAsync(string boxGuid, string currentUserGuid);
        Task<List<IdeaBoxDto>> GetIdeaBoxesAsync(string ideaGuid, string currentUserGuid, int page);
        Task<bool> CreateBoxAsync(string title, string description, bool isAuthored, string authorGuid, string ideaGuid);
        //
        Task<TopicCommentDto> CreateTopicComment(string topicGuid, string authorGuid, string message);
        Task<TopicDetailDto> GetTopicDetail(string topicGuid);
        //
        Task<string> CickMember(string roleGuid);
        Task<bool> AddModder(string roleGuid);
        Task<bool> RemoveModder(string roleGuid);
        //
        Task<List<IdeaRoleDto>> GetIdeaRolesAsync(string ideaGuid);
        Task<bool> CreateTopic(string title, string description, string authorGuid, string ideaGuid);
        //
        IdeaMemberRoleDto GetIdeaMemberRole(string ideaGuid, string userGuid);
        Task<List<IdeaTopicDto>> GetIdeaTopicsAsync(string ideaGuid, int page);
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
