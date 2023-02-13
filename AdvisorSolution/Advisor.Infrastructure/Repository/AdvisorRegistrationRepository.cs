using Advisor.Core.Domain;
using Advisor.Core.Domain.Models;
using Advisor.Core.Interfaces.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Advisor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.Extensions.Configuration;

namespace Advisor.Infrastructure.Repository
{
    public class AdvisorRegistrationRepository : IAdvisorRegistrationRepository
    {
        private readonly AdvisorDbContext _context;
        private readonly IConfiguration _configuration;
        public AdvisorRegistrationRepository(IConfiguration configuration, AdvisorDbContext context)
        {
            _configuration = configuration;
            _context =context;
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

            advisor.VerfiicationTokenForReset = CreateRandomToken();
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




        //when user forgets password we send an email to the user with the token or the url with the token that brings him here
        public string VerifyAdvisor(string token)
        {
            var res = _context.AdvisorDetails.FirstOrDefault(X => X.VerfiicationTokenForReset == token);
            if (res is null)
            {
                return "Invalid Token";
            }
            
            res.VerifiedAt= DateTime.Now;
            _context.SaveChanges();

            return "User verified";//if this message take him to resetting password

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
