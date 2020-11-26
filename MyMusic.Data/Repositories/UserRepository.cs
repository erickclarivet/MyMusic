using Microsoft.EntityFrameworkCore;
using MyMusic.Core.Models;
using MyMusic.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MyMusicDbContext context) 
            : base(context)
        { }

        private MyMusicDbContext MyMusicDbContext
        {
            get { return Context as MyMusicDbContext; }
        }

        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await MyMusicDbContext.Users.SingleOrDefaultAsync(x => x.UserName == username);
            // if user doesn't exist
            if (user == null)
                return null;

            // check if password is correct
            if (!VerificationPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // Authentication successfull
            return user;
        }

        private static bool VerificationPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null)
                throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace.", "password");
            if (storedHash.Length != 64)
                throw new ArgumentException("Invalid lengh of password hash (64 characters expected).", "passwordHash");
            if (storedSalt.Length != 128)
                throw new ArgumentException("Invalid lengh of password hash (128 characters expected)", "passwordHashSalt");
        
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); // Hash the password
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) // Compare with password stored in db
                        return false;
                }
            }
            return true;
        }

        public async Task<User> Create(User user, string password)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password is required");
            
            var resultUser = await MyMusicDbContext.Users.AnyAsync(u => u.UserName == user.UserName);
            if (resultUser)
                throw new Exception(@"Username '{user.UserName}' is already taken.");

            byte[] passwordHash;
            byte[] passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await MyMusicDbContext.Users.AddAsync(user);

            return user;

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key; // Get the key
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); // Hash the password
            }
        }

        public void Delete(int id)
        {
            var user = MyMusicDbContext.Users.Find(id);
            if (user == null)
                throw new Exception(@"User with id '{id}' not found.");

            MyMusicDbContext.Users.Remove(user);
        }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            return await MyMusicDbContext.Users
                .ToListAsync();
        }

        public async Task<User> GetWithUserByIdAsync(int id)
        {
            return await MyMusicDbContext.Users
                 .Where(u => u.Id == id)
                 .FirstOrDefaultAsync();
        }

        public void Update(User userParam, string password = null)
        {
            var user = MyMusicDbContext.Users.Find(userParam.Id);
            if (user == null)
                throw new Exception(@"User with id '{id}' not found.");

            if (userParam.UserName != user.UserName)
            {
                // Check if the new username doesn't already exist
                if (MyMusicDbContext.Users.Any(u => u.UserName == userParam.UserName))
                    throw new Exception(@"Username {userParam} is already taken");
            }

            // Update properties
            user.FirstName = userParam.FirstName;
            user.LastName = userParam.LastName;
            user.UserName = userParam.UserName;

            // Update password if it was enterred
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash;
                byte[] passwordSalt;

                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            MyMusicDbContext.Users.Update(user);
        }
    }
}
