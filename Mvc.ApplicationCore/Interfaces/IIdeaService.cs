using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.Entities.IdeaEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Interfaces
{
    public interface IIdeaService
    {
        Guid Create(CreateIdeaDto model);

    }
}
