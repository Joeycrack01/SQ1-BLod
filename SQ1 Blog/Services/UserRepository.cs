using Microsoft.EntityFrameworkCore;
using SQ1Blog.Entities;
using SQ1Blog.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQ1Blog.Services
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetAdminUser();
        string GetUserById(string userId);
    }

    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser> GetAdminUser()
        {
            var admin = await _context.ApplicationUsers.Where(a => a.IsAdmin == true).FirstOrDefaultAsync();
            return admin;
        }

        public string GetUserById(string userId)
        {
            var admin = _context.ApplicationUsers.Where(a => a.Id == userId).FirstOrDefault();
            return admin.UserName;
        }
    }
}
