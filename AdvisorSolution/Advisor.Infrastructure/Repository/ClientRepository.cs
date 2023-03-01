using Advisor.Core.Domain.DTOs;
using Advisor.Core.Domain.Models;
using Advisor.Core.Interfaces.Repositories;
using Advisor.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    public class ClientRepository:IClientRepository
    {
        private readonly AdvisorDbContext _context;
        private readonly IConfiguration _configuration;
        private static Random random = new Random();
        public ClientRepository(IConfiguration configuration, AdvisorDbContext context, IHttpContextAccessor httpContext) {
            _configuration = configuration;
            _context = context;
        }

        public AdvisorRegisterDTO? CreateClient(AdvisorRegisterDTO request,string email)
        {
            if (_context.Users.Any(X => X.Email == request.Email))
                return null;

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var cliId = CreateAdvisorId();
            var res = _context.Users.FirstOrDefault(X => X.Email == email);
            var advId = res.AdvisorID;
            var advuserid = res.UserID;

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
            advisor.RoleID = 2;
            advisor.AdvisorID = null;
            advisor.ClientID = cliId;
            advisor.AgentID = null;
            advisor.Active = 1;
            advisor.CreatedDate = DateTime.Now;
            advisor.ModifiedBy = advId;
            advisor.ModifiedDate = DateTime.Now;
            advisor.DeletedFlag = 0;
            advisor.VerificationToken = CreateRandomToken();

            _context.Users.Add(advisor);
            _context.SaveChanges();
            var client = _context.Users.FirstOrDefault(X => X.Email == request.Email);
            var clientuserid = client.UserID;
            AdvisorClient ac=new AdvisorClient();
            ac.AdvisorId = res.UserID;
            ac.ClientId = client.UserID;
            _context.AdvisorClients.Add(ac);
            _context.SaveChanges();
            return request;
        }

        private string CreateAdvisorId()
        {
            const string chars = "A1BC2DE3FG5H6I7J4K8L9MN0OPQRSTUVWXYZ";
            var newId = "C" + new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            var res = _context.Users.Any(u => u.AdvisorID == newId);
            if (res == true)
            {
                return CreateAdvisorId();
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

        public string LoginClient(AdvisorLoginDTO request)
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
                 new Claim(ClaimTypes.Role, "client")//user.role
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

        public AdvisorInfoDTO? UpdateClient(AdvisorInfoDTO clientInfo)
        {
            var user = _context.Users.FirstOrDefault(x => x.ClientID == clientInfo.ClientID);
            
            user.LastName = clientInfo.LastName;
            user.FirstName = clientInfo.FirstName;
            user.Address = clientInfo.Address;
            user.City = clientInfo.City;
            user.SortName = clientInfo.LastName + ", " + clientInfo.FirstName;
            user.Company = clientInfo.Company;
            user.Phone = clientInfo.Phone;
            user.State = clientInfo.State;
            _context.Update(user);
            _context.SaveChanges();
            return clientInfo;
        }
    }
}
