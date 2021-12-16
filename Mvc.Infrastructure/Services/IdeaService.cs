using AutoMapper;
using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.Entities.IdeaEntity;
using Mvc.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.Infrastructure.Services
{
    public class IdeaService : IIdeaService
    {
        private IRepository<Idea> _ideaRepository;
        private IRepository<IdeaStatus> _statusRepository;
        private ITagService _tagService;

        public IdeaService(IRepository<Idea> ideaRepo, IRepository<IdeaStatus> status, ITagService tagService)
        {
            _ideaRepository = ideaRepo;
            _statusRepository = status;
            _tagService = tagService;
        }

        public Guid Create(CreateIdeaDto item)
        {
            return Guid.NewGuid();
        }
        
    }
}
