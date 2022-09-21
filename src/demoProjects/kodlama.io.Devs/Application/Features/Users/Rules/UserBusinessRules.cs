using Application.Services.Repositories;
using Core.CrossCuttingConcerns.Exceptions;
using Core.Persistence.Paging;
using Core.Security.Entities;
using Core.Security.Hashing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Rules
{
    public class UserBusinessRules
    {

        private readonly IUserRepository _userRepository;

        public UserBusinessRules(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task EmailCanNotBeDuplicated(string email)
        {
            IPaginate<User> result = await _userRepository.GetListAsync(u => u.Email == email);
            if (result.Items.Any()) throw new BusinessException("This email has already been taken.");

        }

        public void UserEmailShouldBeOnSystemWhenRequested(User user)
        {
            if (user == null) throw new BusinessException("There is no user with given email");
        }

        public void UserPasswordMustBeCorrect(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            HashingHelper.VerifyPasswordHash(password, passwordHash, passwordSalt);
        }
    }
}
