using Application.Features.Users.Dtos;
using Application.Features.Users.Rules;
using Application.Services.AuthService;
using Application.Services.Repositories;
using AutoMapper;
using Core.Mailing;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.Hashing;
using Core.Security.JWT;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.Register
{
    public class RegisterCommand: IRequest<RegisteredDto>
    {
        public UserForRegisterDto? UserForRegisterDto { get; set; }
        public string? IpAddress { get; set; }


        public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisteredDto>
        {
            private readonly IDeveloperRepository _developerRepository;
            private readonly IMapper _mapper;
            private readonly DeveloperBusinessRules _developerBusinessRules;
            private readonly IDeveloperService _developerService;
            private readonly IMailService _mailService;

            public RegisterCommandHandler(IDeveloperRepository developerRepository, IMapper mapper, DeveloperBusinessRules developerBusinessRules, IDeveloperService developerService, IMailService mailService)
            {
                _developerRepository = developerRepository;
                _mapper = mapper;
                _developerBusinessRules = developerBusinessRules;
                _developerService = developerService;
                _mailService = mailService;
            }

            public async Task<RegisteredDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                
                await _developerBusinessRules.EmailCanNotBeDuplicated(request.UserForRegisterDto.Email);

                Byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(request.UserForRegisterDto.Password, out passwordHash, out passwordSalt);

                Developer user = _mapper.Map<Developer>(request.UserForRegisterDto);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Status = true;

                Developer newUser = await _developerRepository.AddAsync(user);

                await _developerService.CreateAndAddUserClaim(newUser);

                RefreshToken refreshToken = await _developerService.CreateRefreshToken(newUser, request.IpAddress);
                AccessToken accessToken = await _developerService.CreateAccessToken(newUser);

                await _developerService.AddRefreshToken(refreshToken);


                RegisteredDto registeredDto = new() { AccessToken = accessToken, RefreshToken = refreshToken };
                _mailService.SendMail(new Mail
                {
                    Subject = "Your account has been created.",
                    TextBody = "Your account created. Have a nice day.",
                    ToEmail = request.UserForRegisterDto.Email,
                    ToFullName = request.UserForRegisterDto.FirstName + " " + request.UserForRegisterDto.LastName
                });

                return registeredDto;

            }
        
            
        }

    }
}
