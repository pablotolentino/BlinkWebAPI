using Blink.Core.Entities;
using Blink.Core.Repositories;
using Blink.Data.Context;
using Blink.Data.Helpers;
using System;

namespace Blink.Data.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private BLINKContext _context { get; set; }
        public AddressRepository(BLINKContext context)
        {
            _context = context;
        }
        public Address Create(Address address)
        {
            _context.Address.Add(address);
            return address;
        }

        public void Delete(int id)
        {
            Address entityToUpdate = _context.Address.Find(id);
            entityToUpdate.Deleted = true;
            entityToUpdate.UpdateDate = DatetimeBo.CurrentDateTime();
            _context.Entry<Address>(entityToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Address.Attach(entityToUpdate);
        }

        public Address Update(Address entityToUpdate)
        {
            Address address = _context.Address.Find(entityToUpdate.AddressId);
            if(address == null)
            {
                throw new Exception("The entity was not found");
            }
            address.CountryId = entityToUpdate.CountryId;
            address.StateId = entityToUpdate.StateId;
            address.MunicipalityId = entityToUpdate.MunicipalityId;
            address.ColonyId = entityToUpdate.ColonyId;
            address.Street = entityToUpdate.Street;
            address.PostalCode = entityToUpdate.PostalCode;
            address.OutdoorNumber = entityToUpdate.OutdoorNumber;
            address.InteriorNumber = entityToUpdate.InteriorNumber;
            address.UpdateDate = DatetimeBo.CurrentDateTime();
            //_context.Address.Attach(address);
            _context.Entry<Address>(address).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Address.Update(address);
            return address;
        }

        public Address Get(long id)
        {
            return _context.Address.Find(id);
        }
    }
}
