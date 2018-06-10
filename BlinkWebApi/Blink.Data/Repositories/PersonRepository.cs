using Blink.Core.Entities;
using Blink.Core.Repositories;
using Blink.Data.Context;
using Blink.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blink.Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        internal BLINKContext _context;
        public PersonRepository(BLINKContext context)
        {
            this._context = context;
        }

        public Person Authenticate(string email,string mobilePhone, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;
            Person user = new Person();

            if (!string.IsNullOrEmpty(email))
            {
            user = _context.Person.SingleOrDefault(x => x.Email == email && x.Deleted == false);
            }
            if (!string.IsNullOrEmpty(mobilePhone))
            {
                user = _context.Person.SingleOrDefault(x => x.MobilePhone == email && x.Deleted == false);

            }

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public Person Create(Person person, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.Person.Any(x => x.Email  == person.Email))
                throw new AppException("Email " + person.Email + " is already taken");

            if (_context.Person.Any(x => x.MobilePhone == person.MobilePhone))
                throw new AppException("MobilePhone " + person.MobilePhone + " is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            person.PasswordHash = passwordHash;
            person.PasswordSalt = passwordSalt;

            person.CreatedDate = DatetimeBo.CurrentDateTime();
            person.Deleted = false;

            Insert(person);
            return person;
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
            Person entityToDelete = _context.Person.Find(id);
            if (entityToDelete == null) return;
            entityToDelete.Deleted = true;
            entityToDelete.UpdateDate = DatetimeBo.CurrentDateTime();
            Update(entityToDelete);
        }
        public void Delete(Person entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _context.Person.Attach(entityToDelete);
            }
            _context.Person.Remove(entityToDelete);
        }

        public Person GetById(int id)
        {
            return _context.Person.Find(id);
        }

        public IEnumerable<Person> GetAll()
        {
            return _context.Person;
        }

        public void Insert(Person entity)
        {
            _context.Person.Add(entity);
        }

        public void Update(Person entityToUpdate)
        {
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            _context.Person.Attach(entityToUpdate);
            _context.Person.Update(entityToUpdate);
        }
        public void Update(Person personParam, string password = null)
        {
            Person person = _context.Person.Find(personParam.PersonId);

            if (person == null)
                throw new AppException("Person not found");

            if (personParam.Email != person.Email)
            {
                // email has changed so check if the new username is already taken
                if (_context.Person.Any(x => x.Email == personParam.Email))
                    throw new AppException("Email " + personParam.Email + " is already taken");
            }

            if (personParam.MobilePhone != person.MobilePhone)
            {
                // mobile phone has changed so check if the new username is already taken
                if (_context.Person.Any(x => x.MobilePhone == personParam.MobilePhone))
                    throw new AppException("MobilePhone " + personParam.MobilePhone + " is already taken");
            }
            //habilitar hasta que sea necesario
            //user.Name = userParam.Name;
            //user.MaternalSurname = userParam.MaternalSurname;
            //user.PaternalSurname = userParam.PaternalSurname;  
            //user.Birthdate = userParam.Birthdate;
            //user.TypeUserId = userParam.TypeUserId;
            //user.TypePersonId = userParam.TypePersonId;
            //user.GenderId = userParam.GenderId;

            // update user properties
            person.MobilePhone = personParam.MobilePhone;
            person.HomePhone = personParam.HomePhone;
            person.UpdateDate = DatetimeBo.CurrentDateTime();
            person.Email = personParam.Email;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                person.PasswordHash = passwordHash;
                person.PasswordSalt = passwordSalt;
            }

            Update(person);
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

        public Person GetByEmail(string email)
        {
            return _context.Person.FirstOrDefault(x=> x.Email.Equals(email));
        }

        public Person GetByMobilePhone(string mobilePhone)
        {
            return _context.Person.FirstOrDefault(x => x.MobilePhone.Equals(mobilePhone));
        }

        public Person GetByRfc(string rfc)
        {
            return _context.Person.FirstOrDefault(x => x.Rfc.Equals(rfc));
        }
    }
}
