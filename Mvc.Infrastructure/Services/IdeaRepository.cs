using Microsoft.AspNetCore.Identity;
using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.Entities;
using Mvc.ApplicationCore.Entities.IdeaEntity;
using Mvc.ApplicationCore.Identity;
using Mvc.ApplicationCore.Interfaces;
using Mvc.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.Infrastructure.Services
{
    public class IdeaRepository : IDisposable, IIdeaRepository
    {
        private readonly ApplicationContext _dbContext;
        private readonly IUserStore<ApplicationUser> _userStore;

        public IdeaRepository(ApplicationContext dbContext, IUserStore<ApplicationUser> userStore)
        {
            _dbContext = dbContext;
            _userStore = userStore;
        }

        public Guid CreateIdea(CreateIdeaDto item)
        {
            throw new NotImplementedException();
        }

        public Task<CommentMessage> CreateCommentAsync(CreateCommentDto item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Idea> GetIdeasPerPage(int page, int count)
        => _dbContext.Ideas
            .Skip((page - 1) * 10)
            .Take(count);

        public IEnumerable<Idea> GetIdeasBySearch(string searchQuery)
        => _dbContext.Ideas
            .Where(x => x.Title.StartsWith(searchQuery) | 
                x.Title.Contains(searchQuery))
            .Take(10);

        public IEnumerable<Idea> GetIdeasByTag(string tagName)
            => _dbContext.Ideas
            .Where(x => x.Tags
            .Any(x => x.Name.Equals(tagName)))
            .Take(10);

        private List<Idea> FillCompletelyRecommends(List<Idea> ideasList)
        {
            if (ideasList.Count < 5)
                ideasList.AddRange(_dbContext.Ideas
                    .Except(ideasList)
                    .Take(5 - ideasList.Count));
            
            return ideasList;
        }

        public async Task<IEnumerable<Idea>> GetUserRecommends(int userId)
        {
            CancellationToken token = new();
            ApplicationUser getUser = await _userStore.FindByIdAsync(userId.ToString(), token);

            List<Idea> recommends = new List<Idea>();
            Tag[] arrayTags = getUser.Skills.ToArray();
            switch (arrayTags.Length)
            {
                case 0:
                    recommends.AddRange(_dbContext.Ideas
                        .Where(x => x.Status
                        .Equals(IdeaStatuses.FindMembers))
                        .Take(5));
                    break;

                case 1:
                    recommends.AddRange(_dbContext.Ideas
                        .Where(x => x.Status
                        .Equals(IdeaStatuses.FindMembers) &&
                            x.Tags.Contains(arrayTags[0]))
                        .Take(5));
                    break;

                case 2:
                    recommends.AddRange(_dbContext.Ideas
                        .Where(x => x.Status
                        .Equals(IdeaStatuses.FindMembers) && 
                            (x.Tags.Contains(arrayTags[0]) | 
                            x.Tags.Contains(arrayTags[1])))
                        .Take(5));
                    break;

                case >= 3:
                    recommends.AddRange(_dbContext.Ideas
                        .Where(x => x.Status
                        .Equals(IdeaStatuses.FindMembers) &&
                            (x.Tags.Contains(arrayTags[0]) |
                            x.Tags.Contains(arrayTags[1]) |
                            x.Tags.Contains(arrayTags[2])))
                        .Take(5));                    
                    break;
            }

            return FillCompletelyRecommends(recommends);
        }

        public IEnumerable<Idea> GetUserRecommends()
            => _dbContext.Ideas
                    .OrderBy(x => x.DateUpdated)
                    .Take(5);

        public async void SaveChanges()
            => await _dbContext.SaveChangesAsync();

        private bool _disposed = false;        
        protected virtual async void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    await _dbContext.DisposeAsync();

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
