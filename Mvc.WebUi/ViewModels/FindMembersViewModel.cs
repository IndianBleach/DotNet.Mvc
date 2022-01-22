using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.DTOs.News;
using Mvc.ApplicationCore.DTOs.User;

namespace Mvc.WebUi.ViewModels
{
    public class FindMembersViewModel
    {
        public List<PageInfoDto> Pages;
        public ICollection<SideUserDto> RecommendUsers { get; set; }
        public ICollection<TagDto> SearchTags {  get; set; }
        public ICollection<TagDto> InterestTags { get; set; }
        public ICollection<UserDto> Users { get; set; }
        public List<NewsDto> News { get; set; }
    }
}