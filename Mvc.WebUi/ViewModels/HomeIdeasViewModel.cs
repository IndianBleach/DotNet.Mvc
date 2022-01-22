using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.DTOs.Idea;
using Mvc.ApplicationCore.DTOs.News;

namespace Mvc.WebUi.ViewModels
{
    public class HomeIdeasViewModel
    {
        public List<PageInfoDto> Pages;
        public ICollection<IdeaRecommendationDto> Recommends { get; set; }
        public ICollection<TagDto> SearchTags { get; set; }
        public ICollection<HomeIdeaDto> Ideas { get; set; }
        public ICollection<SideIdeaDto> IdeasNeedMembers { get; set; }
        public List<NewsDto> News { get; set; }
    }
}
