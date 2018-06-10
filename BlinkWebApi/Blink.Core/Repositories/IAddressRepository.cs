using Blink.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blink.Core.Repositories
{
    public interface IAddressRepository
    {
        Address Create(Address address);
        Address Update(Address entityToUpdate);
        void Delete(int id);
        Address Get(long id);
    }
}
