using Advisor.Core.Domain;
using Advisor.Core.Domain.Models;
using Advisor.Core.Interfaces.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Advisor.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Azure.Core;

namespace Advisor.Infrastructure.Repository
{
    public class AdvisorRegistrationRepository : IAdvisorRegistrationRepository
    {
        private readonly AdvisorDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;
        public AdvisorRegistrationRepository(IConfiguration configuration, AdvisorDbContext context, IHttpContextAccessor httpContext)
        {
            _configuration = configuration;
            _context =context;
            _httpContext = httpContext;
        }
        public AdvisorRegistrationDetails CreateUser(AdvisorDTO request)
        {
            if( _context.AdvisorDetails.Any(X => X.Email == request.Email))
                return null;

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            AdvisorRegistrationDetails advisor = new AdvisorRegistrationDetails();
            advisor.Address = request.Address;
            advisor.Email = request.Email;
            advisor.Phone = request.Phone;
            advisor.Name = request.Name;
            advisor.AdvisroId = request.AdvisroId;
            advisor.Company = request.Company;
            advisor.City = request.City;
            advisor.State = request.State;
            advisor.Password = request.Password;
            advisor.PasswordHash = passwordHash;
            advisor.PasswordSalt = passwordSalt;
            _context.AdvisorDetails.Add(advisor);
            _context.SaveChanges();
            return advisor;
        }
        public string CreateRandomToken() {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
        public string LoginAdvisor(AdvisorLoginDTO request)
        {
            var res=_context.AdvisorDetails.FirstOrDefault(X => X.Email == request.Email);
            if (res is null) {
                return "Email doesn't exist.";
            }
            if (!VerifyPasswordHash(request.Password, res.PasswordHash, res.PasswordSalt))
                return "Wrong password.";

            string token = CreateToken(res);
            return token;

        }

        public string ForgotPassword(string email)
        {
            var user = _context.AdvisorDetails.FirstOrDefault(x => x.Email == email);
            if (user is null)
            {
                return "Bad Request.";
            }
            user.PasswordResetToken = CreateRandomToken();
            user.ResetTokenExpires=DateTime.Now.AddDays(1);
            _context.SaveChanges();
            //on clicking forgot password then the page will redirect to a form the form should be submitted with in one day else the token will expire and when they click submit
            //in the post request the datetime of the request should be included
            return user.PasswordResetToken;

        }

        public string ResetPassword(PasswordResetDTO reset)
        {
            var email = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Email).ToString();
            
            string[] words = email.Split(' ');
            email = words[1];
 
            var user = _context.AdvisorDetails.FirstOrDefault(x => x.Email == email);
            if (reset.now > user.ResetTokenExpires) {
                return "Session expired.";
            }
            user.Password= reset.Password;
            CreatePasswordHash(reset.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordSalt=passwordSalt;
            user.PasswordHash=passwordHash;
            _context.SaveChanges();

            return "Password updated.";
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(AdvisorRegistrationDetails user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role, "advisor")//user.role
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
