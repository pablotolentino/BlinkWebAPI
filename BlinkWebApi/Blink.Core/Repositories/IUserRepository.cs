using Blink.Core.Entities;
using System.Collections.Generic;

namespace Blink.Core.Repositories
{
    public interface IUserRepository
    {
        User Create(User user, string password);
        void Update(User userParam, string password = null);
        void Update(User entityToUpdate);
        void Delete(int id);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Authenticate(string username, string password);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
        void Delete(User entityToDelete);
        void Insert(User entity);        
    }
}
