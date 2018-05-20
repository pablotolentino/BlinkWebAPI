using Blink.Core.Entities;
using Blink.Core.Repositories;
using Blink.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blink.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        internal BLINKContext context;
        public UserRepository(BLINKContext context)
        {
            this.context = context;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = context.User.SingleOrDefault(x => x.Username == username && x.Deleted == false);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public User Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (context.User.Any(x => x.Username == user.Username))
                throw new AppException("Username " + user.Username + " is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            user.CreatedDate = DatetimeBo.CurrentDateTime();
            user.Deleted = false;

            Insert(user);
            return user;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public void Delete(int id)
        {
            User entityToDelete = context.User.Find(id);
            if (entityToDelete == null) return;
            entityToDelete.Deleted = true;
            entityToDelete.UpdateDate = DatetimeBo.CurrentDateTime();
            Update(entityToDelete);
        }
        public void Delete(User entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                context.User.Attach(entityToDelete);
            }
            context.User.Remove(entityToDelete);
        }

        public User GetById(int id)
        {
            return context.User.Find(id);
        }

        public IEnumerable<User> GetAll()
        {
            return context.User;
        }

        public void Insert(User entity)
        {            
            context.User.Add(entity);
        }

        public void Update(User entityToUpdate)
        {            
            context.Entry(entityToUpdate).State = EntityState.Modified;
            context.User.Attach(entityToUpdate);
        }
        public void Update(User userParam, string password = null)
        {
            var user = context.User.Find(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            if (userParam.Username != user.Username)
            {
                // username has changed so check if the new username is already taken
                if (context.User.Any(x => x.Username == userParam.Username))
                    throw new AppException("Username " + userParam.Username + " is already taken");
            }

            // update user properties
            user.Name = userParam.Name;
            user.MaternalSurname = userParam.Name;
            user.Username = userParam.Username;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }
            user.UpdateDate = DatetimeBo.CurrentDateTime();
           
            Update(user);
        }
        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

    }
}
