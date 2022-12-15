using Application.Features.Users.Constants;
using Application.Features.Users.Dtos;
using Application.Features.Users.Rules;
using Application.Services.AuthService;
using Application.Services.Repositories;
using Core.CrossCuttingConcerns.Exceptions;
using Core.Security.Entities;
using Core.Security.JWT;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.Refresh
{
    public class RefreshTokenCommand: IRequest<RefreshedTokenDto>
    {
        public RefreshTokenDto RefreshTokenDto { get; set; }
        public string? IpAddress { get; set; }

        public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshedTokenDto>
        {
            private readonly IRefreshTokenRepository _refreshTokenRepository;
            private readonly IDeveloperRepository _developerRepository;
            private readonly IAuthService _developerService;
            private readonly AuthBusinessRules _developerBusinessRules;

            public RefreshTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository, IDeveloperRepository developerRepository, IAuthService developerService, AuthBusinessRules developerBusinessRules)
            {
                _refreshTokenRepository = refreshTokenRepository;
                _developerRepository = developerRepository;
                _developerService = developerService;
                _developerBusinessRules = developerBusinessRules;
            }

            public async Task<RefreshedTokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
            {
                _developerService.IsTokenValid(request.RefreshTokenDto.AccessToken);
                //_developerBusinessRules.AccessTokenMustBeExpiredToRefresh(request.RefreshTokenDto.AccessToken);

                ClaimsPrincipal claims = _developerService.GetPrincipleFromToken(request.RefreshTokenDto.AccessToken);


                RefreshToken? storedRefreshToken = await _refreshTokenRepository.GetAsync(x => x.Token == request.RefreshTokenDto.RefreshToken);
                _developerBusinessRules.RefreshTokenMustBeExistWhenRequested(storedRefreshToken);

                _developerBusinessRules.RefreshTokenMustNotBeExpireated(storedRefreshToken);
                _developerBusinessRules.RefreshTokenAndAccessTokenMustOwnSameUser(storedRefreshToken, request.RefreshTokenDto.AccessToken);

                Developer? developer = await _developerRepository.GetAsync(d => d.Id == storedRefreshToken.UserId);
                _developerBusinessRules.DeveloperMustBeExistWhenRequested(developer);

                AccessToken newAccessToken = await _developerService.CreateAccessToken(developer);
                RefreshToken newRefreshToken = await _developerService.CreateRefreshToken(developer, request.IpAddress);

                await _developerService.AddRefreshToken(newRefreshToken);

                await _developerService.RevokeRefreshToken(storedRefreshToken, request.IpAddress, newRefreshToken.Token, AuthMessages.RevokedForRefresh);

                RefreshedTokenDto refreshedTokenDto = new()
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                };

                return refreshedTokenDto;






            }
        }
    }
}
