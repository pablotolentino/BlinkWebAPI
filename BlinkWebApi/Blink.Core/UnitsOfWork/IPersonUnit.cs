using Blink.Core.Entities;
using Blink.Core.Repositories;

namespace Blink.Core.UnitsOfWork
{
   public interface IPersonUnit: IUnitOfWork
    {
        IPersonRepository PersonRepository { get; }
        IAddressRepository AddressRepository { get; }
        IGenericRepository<BinnacleAddress> BinnacleAddressRepository { get; }
    }
}
