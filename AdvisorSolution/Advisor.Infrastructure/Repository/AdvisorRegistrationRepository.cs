using Advisor.Core.Domain.Models;
using Advisor.Core.Interfaces.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Advisor.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Advisor.Core.Domain.DTOs;
using Advisor.Core.Domain;

namespace Advisor.Infrastructure.Repository
{
    public class AdvisorRegistrationRepository : IAdvisorRegistrationRepository
    {
        private readonly AdvisorDbContext _context;
        private readonly IConfiguration _configuration;
        
        private static Random random = new Random();

        public AdvisorRegistrationRepository(IConfiguration configuration, AdvisorDbContext context, IHttpContextAccessor httpContext)
        {
            _configuration = configuration;
            _context = context;
            
        }


        public AdvisorRegisterDTO? CreateAdvisor(AdvisorRegisterDTO request)
        {
            if (_context.Users.Any(X => X.Email == request.Email))
                return null;

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var advId = CreateAdvisorId();
            Users advisor = new Users();
            advisor.Address = request.Address;
            advisor.Email = request.Email;
            advisor.Phone = request.Phone;
            advisor.Company = request.Company;
            advisor.City = request.City;
            advisor.State = request.State;
            advisor.PasswordHash = passwordHash;
            advisor.PasswordSalt = passwordSalt;
            advisor.FirstName = request.FirstName;
            advisor.LastName = request.LastName;
            advisor.SortName = request.LastName + ", " + request.FirstName;
            advisor.RoleID = 1;
            advisor.AdvisorID = advId;
            advisor.ClientID = null;
            advisor.AgentID = null;
            advisor.Active = 1;
            advisor.CreatedDate = DateTime.Now;
            advisor.ModifiedBy = advId;
            advisor.ModifiedDate = DateTime.Now;
            advisor.DeletedFlag = 0;
            advisor.VerificationToken = CreateRandomToken();

            _context.Users.Add(advisor);
            _context.SaveChanges();
            return request;
        }


        private string CreateAdvisorId()
        {
            const string chars = "a1bc2de3fg5h6i7j4k8l9mn0opqrstuvwxyz";
            var newId = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            var res = _context.Users.Any(u => u.AdvisorID == newId);
            if (res == true)
            {
                CreateAdvisorId();
            }
            return newId;
        }

        public string CreateRandomToken()
        {
            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
            if (_context.Users.Any(x => x.AdvisorID == token))
                token = CreateRandomToken();
            return token;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


        public string LoginAdvisor(AdvisorLoginDTO request)
        {
            var res = _context.Users.FirstOrDefault(X => X.Email == request.Email);
            if (res is null)
                return "Email doesn't exist.";

            if (!VerifyPasswordHash(request.Password, res.PasswordHash, res.PasswordSalt))
                return "Wrong password.";

            string token = CreateToken(res);
            return token;

        }


        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateToken(Users user)
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


        public string ChangePasswordAdv(string email)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email);
            if (user is null)
            {
                return "Bad Request.";
            }
            user.PasswordResetToken = CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddDays(1);
            _context.SaveChanges();
            //on clicking change password then the page will redirect to a form the form should be submitted with in one day else the token will expire and when they click submit
            //in the post request the datetime of the request should be included
            return user.PasswordResetToken;
        }

        public string ResetPasswordAdvAfterLogin(PasswordResetDTO reset, string email)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email);
            if (reset.now > user.ResetTokenExpires)
                return "Session expired.";

            if (!reset.token.Equals(user.PasswordResetToken))
                return "Not authorized.";

            CreatePasswordHash(reset.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;
            _context.SaveChanges();

            return "Password updated.";
        }

        public string ForgotPassword(PasswordResetWithoutLoginDTO request)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == request.Email);
            if (user is null)
                return "No User with this email exists.";
            user.PasswordResetToken = CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddDays(1);
            _context.SaveChanges();
            //on clicking forgot password then the page will redirect to a form the form should be submitted with in one day else the token will expire and when they click submit
            //in the post request the datetime of the request should be included
            return user.PasswordResetToken;
            //iske baad call after login wala
        }

        public AdvisorInfoDTO? GetAdvisorInfo(string email)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email);
            if (user is null)
                return null;
            AdvisorInfoDTO advisorInfo = new AdvisorInfoDTO();
            advisorInfo.Email = email;
            advisorInfo.LastName = user.LastName;
            advisorInfo.FirstName = user.FirstName;
            advisorInfo.AdvisorID = user.AdvisorID;
            advisorInfo.Address = user.Address;
            advisorInfo.City = user.City;
            advisorInfo.Company = user.Company;
            advisorInfo.Phone = user.Phone;
            advisorInfo.State = user.State;
            return advisorInfo;
        }

        public List<AdvisorInfoDTO> GetAllAdvisors()
        {
            List<AdvisorInfoDTO> users = new List<AdvisorInfoDTO>();

            foreach (var user in _context.Users)
            {
                AdvisorInfoDTO advisorInfo = new AdvisorInfoDTO();
                advisorInfo.Email = user.Email;
                advisorInfo.LastName = user.LastName;
                advisorInfo.FirstName = user.FirstName;
                advisorInfo.AdvisorID = user.AdvisorID;
                advisorInfo.Address = user.Address;
                advisorInfo.City = user.City;
                advisorInfo.Company = user.Company;
                advisorInfo.Phone = user.Phone;
                advisorInfo.State = user.State;
                users.Add(advisorInfo);
            }

            return users;
        }

        public AdvisorInfoDTO UpdateAdvisor(string email, AdvisorInfoDTO advisorInfo)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email);
            user.Email = advisorInfo.Email;
            user.LastName = advisorInfo.LastName;
            user.FirstName = advisorInfo.FirstName;
            user.AdvisorID = advisorInfo.AdvisorID;
            user.Address = advisorInfo.Address;
            user.City = advisorInfo.City;
            user.SortName= advisorInfo.LastName + ", " + advisorInfo.FirstName;
            user.Company = advisorInfo.Company;
            user.Phone = advisorInfo.Phone;
            user.State = advisorInfo.State;
            _context.Update(user);
            _context.SaveChanges();
            return advisorInfo;
        }

        public List<AdvisorInfoDTO> DeleteUser(string email)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email);
            _context.Users.Remove(user);
            _context.SaveChanges();
            return GetAllAdvisors();
        }
    }
}