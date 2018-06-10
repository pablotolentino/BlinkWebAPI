using Blink.Core.Entities;
using Blink.Core.Repositories;
using Blink.Core.UnitsOfWork;
using Blink.Data.Context;
using Blink.Data.Repositories;

namespace Blink.Data.UnitsOfWork
{
    public class PersonUnit : IPersonUnit
    {
        private BLINKContext _context { get; set; }
        public PersonUnit(BLINKContext context)
        {
            this._context = context;
            this.PersonRepository = new PersonRepository(context);
            this.AddressRepository = new AddressRepository(context);
            this.BinnacleAddressRepository = new GenericRepository<BinnacleAddress>(context);
        }
        public IPersonRepository PersonRepository { get; private set; }
        public IAddressRepository AddressRepository { get; private set; }

        public IGenericRepository<BinnacleAddress> BinnacleAddressRepository { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
