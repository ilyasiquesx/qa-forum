using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using QAForum.Application.Common.Interfaces;
using QAForum.Application.Common.Models;
using QAForum.Domain.Entities;
using QAForum.Infrastructure.Context;

namespace QAForum.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public IdentityService(UserManager<AppUser> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, AppUser>()
                    .ForMember
                    (dest => dest.User,
                        opt => opt.MapFrom(u => u));
            });
            _mapper = new Mapper(mapperCfg);
        }

        public async Task<RegisterResult> CreateUserAsync(User user, string password)
        {
            await using var transaction = await _appDbContext.Database.BeginTransactionAsync();
            IdentityResult identityResult;
            try
            {
                user.Id = Guid.NewGuid().ToString();
                _appDbContext.Users.Add(user);
                await _appDbContext.SaveChangesAsync();

                var identityUser = _mapper.Map<AppUser>(user);
                identityResult = await _userManager.CreateAsync(identityUser, password);

                if (identityResult.Succeeded)
                {
                    await transaction.CommitAsync();
                }
                else
                {
                    await transaction.RollbackAsync();
                }
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return new RegisterResult
                {
                    IsSucceeded = false,
                    Errors = new List<string>
                    {
                        e.Message
                    }
                };
            }

            return new RegisterResult
            {
                IsSucceeded = identityResult.Succeeded,
                Errors = identityResult.Errors.Select(e => e.Description)
            };
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var identityUser = await _userManager.FindByNameAsync(username);
            return identityUser?.User;
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var identityUser = await _userManager.FindByIdAsync(user.Id);
            return await _userManager.CheckPasswordAsync(identityUser, password);
        }
    }
}