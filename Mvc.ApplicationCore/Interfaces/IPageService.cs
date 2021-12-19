using Mvc.ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Interfaces
{
    public interface IPageService
    {
        List<PageInfoDto> GeneratePages(int currentPage, int totalCount, int orderByCount);
    }
}
