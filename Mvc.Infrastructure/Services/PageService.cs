using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.Infrastructure.Services
{
    public class PageService : IPageService
    {
        public List<PageInfoDto> GeneratePages(int currentPage, int totalCount, int orderByCount)
        {
            List<PageInfoDto> pageInfoDtos = new List<PageInfoDto>();

            int countPages = (totalCount / orderByCount ) + 1;

            if (totalCount % orderByCount == 0) countPages--;

            int generatePagesCount = countPages > 2 ? 3 : 2;

            if (countPages <= 1) pageInfoDtos.Add(new(true, currentPage));
            else if (currentPage == countPages)
            {
                if (countPages > 2)
                {
                    pageInfoDtos.Add(new(false, currentPage - 2));
                    pageInfoDtos.Add(new(false, currentPage - 1));
                    pageInfoDtos.Add(new(true, currentPage));
                }
                else
                {
                    pageInfoDtos.Add(new(false, currentPage - 1));
                    pageInfoDtos.Add(new(true, currentPage));
                }

            }
            else
            {
                if (countPages > 2)
                {
                    if (currentPage == 1)
                    {
                        pageInfoDtos.Add(new(true, currentPage));
                        pageInfoDtos.Add(new(false, currentPage + 1));
                        pageInfoDtos.Add(new(false, currentPage + 2));
                    }
                    else
                    {
                        pageInfoDtos.Add(new(false, currentPage - 1));
                        pageInfoDtos.Add(new(true, currentPage));
                        pageInfoDtos.Add(new(false, currentPage + 1));
                    }
                }
                else
                {
                    pageInfoDtos.Add(new(true, currentPage));
                    pageInfoDtos.Add(new(false, currentPage + 1));
                }
            }

            return pageInfoDtos;
        }
    }
}
