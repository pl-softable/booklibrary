namespace BookLibrary.Identity.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Models;
    using Core.Queries;
    using Core.Queries.Identity;
    using DataAccess.Contexts;
    using DataAccess.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    public class GetTokenHandler : IQueryHandler<GetTokenQuery, TokenResponse>
    {
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<Librarian> userManager;
        private readonly SignInManager<Librarian> signInManager;

        public GetTokenHandler(IConfiguration configuration, ApplicationDbContext dbContext,
            UserManager<Librarian> userManager, SignInManager<Librarian> signInManager)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<TokenResponse> Handle(GetTokenQuery request, CancellationToken cancellationToken)
        {
            var result = await this.signInManager.PasswordSignInAsync(
                request.Email, request.Password, false, false);

            if (!result.Succeeded) throw new Exception("Invalid credentials");

            var token = await TokenResponse(request);

            return token;
        }

        private async Task<TokenResponse> TokenResponse(GetTokenQuery request)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(this.configuration["JwtSecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(this.configuration["JwtExpiryInDays"]));

            var user = await this.dbContext.Users.FirstOrDefaultAsync(item => item.UserName == request.Email);

            var roles = await this.userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, request.Email)
            };

            claims.AddRange(roles.Select(item => new Claim(ClaimTypes.Role, item)).ToArray());

            var token = new JwtSecurityToken(
                this.configuration["JwtIssuer"],
                this.configuration["JwtAudience"],
                claims,
                expires: expiry,
                signingCredentials: creds
            );

            var response = new TokenResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };

            return response;
        }
    }
}