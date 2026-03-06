using Assignment.DTOs;
using Assignment.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project_Bootcamp_2025.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(UserManager<Candidate> accountUser, IConfiguration configuration) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CandidateCDTO model)
        {
            var existedUser = await accountUser.FindByNameAsync(model.UserName);
            if (existedUser != null)
            {
                ModelState.AddModelError("error", "User name is already taken.");
                return BadRequest(ModelState);
            }
            var user = model.Adapt<Candidate>();
            user.SecurityStamp = Guid.NewGuid().ToString();
            //try to save user
            var result = await accountUser.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                //try to assign role
                var roleresult = await accountUser.AddToRoleAsync(user, AppRoles.User);
                if (roleresult.Succeeded)
                {
                    return Ok();
                }
            }
            //if there is are errors, add then to the ModelState object
            //and return the error to the client
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("error", error.Description);
            }
            return BadRequest(ModelState);

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var user = await accountUser.FindByNameAsync(model.UserName);
            if (user != null)
            {
                if (await accountUser.CheckPasswordAsync(user, model.Password))
                {
                    var token = GenerateToken(user, model.UserName);
                    var Id = user.Id;
                    return Ok(new { token });
                }
            }
            ModelState.AddModelError("error", "Invalid username or password");
            return BadRequest(ModelState);
        }
        private async Task<string?> GenerateToken(Candidate model, string userName)
        {
            var secret = configuration["JwtConfig:Secret"];
            var issuer = configuration["JwtConfig:ValidIssuer"];
            var audience = configuration["JwtConfig:ValidAudiences"];
            if (secret is null || audience is null || issuer is null)
            {
                throw new ApplicationException("Jwt is not set in the configuration");
            }
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var tokenHandler = new JwtSecurityTokenHandler();
            //add roles as claims
            var userRoles = await accountUser.GetRolesAsync(model);
            var claims = new List<Claim>
            {
                new (ClaimTypes.Name, userName)
            };
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //generate token based on roles
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return token;
        }
    }
}