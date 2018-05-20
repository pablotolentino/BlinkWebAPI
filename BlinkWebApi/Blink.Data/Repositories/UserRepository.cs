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
        internal BLINKContext _context;
        public UserRepository(BLINKContext context)
        {
            this._context = context;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            User user = _context.User.SingleOrDefault(x => x.Username == username && x.Deleted == false);

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

            if (_context.User.Any(x => x.Username == user.Username))
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
            User entityToDelete = _context.User.Find(id);
            if (entityToDelete == null) return;
            entityToDelete.Deleted = true;
            entityToDelete.UpdateDate = DatetimeBo.CurrentDateTime();
            Update(entityToDelete);
        }
        public void Delete(User entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _context.User.Attach(entityToDelete);
            }
            _context.User.Remove(entityToDelete);
        }

        public User GetById(int id)
        {
            return _context.User.Find(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.User;
        }

        public void Insert(User entity)
        {            
            _context.User.Add(entity);
        }

        public void Update(User entityToUpdate)
        {            
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            _context.User.Attach(entityToUpdate);
        }
        public void Update(User userParam, string password = null)
        {
            User user = _context.User.Find(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            if (userParam.Username != user.Username)
            {
                // username has changed so check if the new username is already taken
                if (_context.User.Any(x => x.Username == userParam.Username))
                    throw new AppException("Username " + userParam.Username + " is already taken");
            }

            // update user properties
            user.Name = userParam.Name;
            user.MaternalSurname = userParam.MaternalSurname;
            user.PaternalSurname = userParam.PaternalSurname;
            user.Username = userParam.Username;
            user.Birthdate = userParam.Birthdate;
            user.Gender = userParam.Gender;
            user.HomePhone = userParam.HomePhone;
            user.MobilePhone = userParam.MobilePhone;
            user.TypeUser = userParam.TypeUser;
            user.IsPhysical = userParam.IsPhysical;
            user.UpdateDate = DatetimeBo.CurrentDateTime();
            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }           
           
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
