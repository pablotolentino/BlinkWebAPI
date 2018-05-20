using Blink.Core.Repositories;

namespace Blink.Core.UnitsOfWork
{
   public interface IUserUnit: IUnitOfWork
    {
        IUserRepository UserRepository { get; }
    }
}
