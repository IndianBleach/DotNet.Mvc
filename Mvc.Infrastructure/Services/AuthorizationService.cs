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


        public async Task LoginAsync(UserLoginDto model)
        {
            var findUser = await _userManager.FindByNameAsync(model.Username);

            var isValidPassword = await _userManager.CheckPasswordAsync(findUser, model.Password);

            if (isValidPassword)
            {
                await _signInManager.SignInAsync(findUser, false);
            }
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task RegisterAsync(UserRegisterDto model)
        {
            var config = new MapperConfiguration(conf => conf.CreateMap<UserRegisterDto, ApplicationUser>()
            .ForMember("UserName", opt => opt.MapFrom(x => x.Username))
            .ForMember("Skills", opt => opt.MapFrom(x => _tagService.CreateTagList(x.Tags))));

            var mapper = new Mapper(config);

            ApplicationUser createUser = mapper.Map<ApplicationUser>(model);

            var result = await _userManager.CreateAsync(createUser, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(createUser, "user");
                await _signInManager.SignInAsync(createUser, false);
            }            
        }
    }
}
