﻿using AuthIdentityJWT.Core.Configuration;
using AuthIdentityJWT.Core.Dtos;
using AuthIdentityJWT.Core.Models;
using AuthIdentityJWT.Core.Repositories;
using AuthIdentityJWT.Core.Services;
using AuthIdentityJWT.Core.UnitOfWork;
using AuthIdentityJWT.SharedLibrary.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuthIdentityJWT.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _refreshTokenService;
        public AuthenticationService(IOptions<List<Client>> optionsClient, ITokenService tokenService, UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> refreshTokenService)
        {
            _clients = optionsClient.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _refreshTokenService = refreshTokenService;
        }
        public async Task<Response<TokenDto>> CreateToken(SignInDto signInDto)
        {
            if (signInDto == null) throw new ArgumentNullException(nameof(signInDto));
            var user = await _userManager.FindByEmailAsync(signInDto.EMail);
            if (user == null) return Response<TokenDto>.Fail("Email or Password is wrong", 400, true);
            if (!await _userManager.CheckPasswordAsync(user, signInDto.Password)) { return Response<TokenDto>.Fail("Email or Password is wrong", 400, true); }
            var token = _tokenService.CreateToken(user);
            var userRefreshToken = await _refreshTokenService.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();
            if (userRefreshToken == null)
            {
                await _refreshTokenService.AddAsync(new UserRefreshToken { UserId = user.Id, Code = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
            }
            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }
            await _unitOfWork.CommitAsync();
            return Response<TokenDto>.Success(token, 200);
        }

        public Response<ClientTokenDto> CreateTokenByClient(ClientSignInDto clientSignInDto)
        {
            var client = _clients.SingleOrDefault(x => x.Id == clientSignInDto.ClientId && x.Secret == clientSignInDto.ClientSecret);
            if (client == null)
            {
                return Response<ClientTokenDto>.Fail("ClientId or ClientSecret not found", 400, true);
            }
            var token = _tokenService.CreateTokenByClient(client);
            return Response<ClientTokenDto>.Success(token, 200);


        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _refreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
            if (existRefreshToken == null)
            {
                return Response<TokenDto>.Fail("Refresh token not found", 404, true);
            }
            var user = await _userManager.FindByIdAsync(existRefreshToken.UserId);
            if (user == null)
            {
                return Response<TokenDto>.Fail("User Id not found", 404, true);
            }
            var tokenDto = _tokenService.CreateToken(user);
            existRefreshToken.Code = tokenDto.RefreshToken;
            existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;
            await _unitOfWork.CommitAsync();
            return Response<TokenDto>.Success(tokenDto, 200);
        }

        public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _refreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
            if (existRefreshToken == null)
            {
                return Response<NoDataDto>.Fail("Refresh token not found", 404, true);
            }
            _refreshTokenService.Remove(existRefreshToken);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(200);
        }
    }
}
