using Blink.Core.Repositories;
using Blink.Core.UnitsOfWork;
using Blink.Data.Repositories;

namespace Blink.Data.UnitsOfWork
{
    public class UserUnit : IUserUnit
    {
        private BLINKContext _context { get; set; }
        public UserUnit(BLINKContext context)
        {
            this._context = context;
            this.UserRepository = new UserRepository(context);
        }
        public IUserRepository UserRepository { get; private set; }

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
