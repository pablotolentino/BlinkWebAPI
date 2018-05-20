using System;

namespace Blink.Core.UnitsOfWork
{
    public interface IUnitOfWork: IDisposable
    {       
        void SaveChanges();
    }
}
