using Core.Security.Enums;
using Core.Security.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Dtos
{
    public class LoggedInUserDto: RefreshedTokenDto
    {
        public AuthenticatorType RequiredAuthenticatorType { get; set; }

        public LoggedUserResponseDto CreateResponseDto()
        {
            return new() { AccessToken = AccessToken, RequiredAuthenticatorType = RequiredAuthenticatorType };
        }
        public class LoggedUserResponseDto
        {
            public AccessToken AccessToken{ get; set; }
            public AuthenticatorType RequiredAuthenticatorType{ get; set; }
        }
    }
}
