﻿using Mvc.ApplicationCore.DTOs;
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
        IEnumerable<HomeIdeaDto> GetIdeasPerPage(int page);
        void Save();
        Guid CreateIdea(CreateIdeaDto item);        
    }
}
