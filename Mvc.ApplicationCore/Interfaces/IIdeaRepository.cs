using Mvc.ApplicationCore.DTOs;
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
        //Guid CreateIdea(CreateIdeaDto item);
        
        //Task<CommentMessage> CreateCommentAsync(CreateCommentDto item);

        IEnumerable<Idea> GetIdeasPerPage(int page, int count);

        IEnumerable<Idea> GetIdeasBySearch(string searchQuery);

        IEnumerable<Idea> GetIdeasByTag(string tagName);

        Task<IEnumerable<Idea>> GetUserRecommends(int userId);

        IEnumerable<Idea> GetUserRecommends();
    }
}
