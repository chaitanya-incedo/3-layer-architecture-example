using Advisor.Core.Domain.DTOs;
using Advisor.Core.Domain.Models;
using Advisor.Core.Interfaces.Repositories;
using Advisor.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Advisor.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AdvisorDbContext _context;
        private readonly IConfiguration _configuration;
        private static Random random = new Random();
        public UserRepository(IConfiguration configuration, AdvisorDbContext context, IHttpContextAccessor httpContext)
        {
            _configuration = configuration;
            _context = context;
        }
        public string? CreateAdvisor(RegistrationDTO request)
        {
            if (_context.Users.Any(X => X.Email == request.Email))
                return "Email already exists";

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
            advisor.RoleID = request.RoleID;
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
            return "user registered";
        }
                                                                            
                                                                            private string CreateClientId()
                                                                            {
                                                                                const string chars = "A1BC2DE3FG5H6I7J4K8L9MN0OPQRSTUVWXYZ";
                                                                                var newId = "C" + new string(Enumerable.Repeat(chars, 5)
                                                                                    .Select(s => s[random.Next(s.Length)]).ToArray());
                                                                                var res = _context.Users.Any(u => u.ClientID == newId);
                                                                                if (res == true)
                                                                                {
                                                                                    CreateClientId();
                                                                                }
                                                                                return newId;
                                                                            }
                                                                            private string CreateAdvisorId()
                                                                            {
                                                                                const string chars = "A1BC2DE3FG5H6I7J4K8L9MN0OPQRSTUVWXYZ";
                                                                                var newId = "A" + new string(Enumerable.Repeat(chars, 5)
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

        public string? CreateClient(RegistrationDTO request, string AdvisorID)
        {
            if (_context.Users.Any(X => X.Email == request.Email))
                return "Email already exists";

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var cliId = CreateClientId();
            var res = _context.Users.FirstOrDefault(X => X.AdvisorID == AdvisorID);
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
            advisor.RoleID = request.RoleID;
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
            AdvisorClient ac = new AdvisorClient();
            ac.AdvisorId = res.UserID;
            ac.ClientId = client.UserID;
            _context.AdvisorClients.Add(ac);
            _context.SaveChanges();
            return "user registered";
        }

        public string? UpdateClient(AdvisorInfoDTO clientInfo, string ClientId)
        {
            var user = _context.Users.FirstOrDefault(x => x.ClientID == ClientId);
            if (user == null)
                return "No such user exists inxorrect ClientID";
            user.Email = clientInfo.Email;
            user.LastName = clientInfo.LastName;
            user.FirstName = clientInfo.FirstName;
            user.AdvisorID = clientInfo.AdvisorID;
            user.Address = clientInfo.Address;
            user.City = clientInfo.City;
            user.SortName = clientInfo.LastName + ", " + clientInfo.FirstName;
            user.Company = clientInfo.Company;
            user.Phone = clientInfo.Phone;
            user.State = clientInfo.State;
            _context.Update(user);
            _context.SaveChanges();
            return "Client Updated";
        }
    }
}
