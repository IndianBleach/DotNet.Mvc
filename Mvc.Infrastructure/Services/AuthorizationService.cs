using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Mvc.ApplicationCore.DTOs;
using Mvc.ApplicationCore.Entities;
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
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly ApplicationContext _dbContext;
        private readonly ITagService _tagService;


        public AuthorizationService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITagService tagService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tagService = tagService;
        }


        public Task LoginAsync(UserLoginDto model)
        {            
            throw new NotImplementedException();
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public async Task RegisterAsync(UserRegisterDto model)
        {
            //

            var config = new MapperConfiguration(conf => conf.CreateMap<UserRegisterDto, ApplicationUser>()
            .ForMember("UserName", opt => opt.MapFrom(x => x.Username))
            .ForMember("PasswordHash", opt => opt.MapFrom(x => x.Password))
            .ForMember("Skills", opt => opt.MapFrom(x => _tagService.CreateTagList(x.Tags))));

            var mapper = new Mapper(config);

            ApplicationUser createUser = mapper.Map<ApplicationUser>(model);

            await _signInManager.SignInAsync(createUser, false);
        }
    }
}
