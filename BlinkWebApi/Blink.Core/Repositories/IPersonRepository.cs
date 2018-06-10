using Blink.Core.Entities;
using System.Collections.Generic;

namespace Blink.Core.Repositories
{
    public interface IPersonRepository
    {
        Person Create(Person user, string password);
        void Update(Person userParam, string password = null);
        void Update(Person entityToUpdate);
        void Delete(int id);
        IEnumerable<Person> GetAll();
        Person GetById(int id);
        Person Authenticate(string username, string mobilePhone, string password);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
        void Delete(Person entityToDelete);
        void Insert(Person entity); 
        Person GetByEmail(string email);
        Person GetByMobilePhone(string mobilePhone);
        Person GetByRfc(string rfc);
    }
}
