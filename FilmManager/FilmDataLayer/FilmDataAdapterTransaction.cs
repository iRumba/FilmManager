using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmDataLayer
{
    public class FilmDataAdapterTransaction : IDisposable
    {
        DbContextTransaction _sourceTransaction;

        internal FilmDataAdapterTransaction(DbContextTransaction sourceTransaction)
        {
            _sourceTransaction = sourceTransaction;
        }

        public void Rollback()
        {
            _sourceTransaction.Rollback();
        }

        public void Commit()
        {
            _sourceTransaction.Commit();
        }

        public void Dispose()
        {
            try
            {
                _sourceTransaction.Commit();
            }
            catch
            {
                _sourceTransaction.Rollback();
                throw;
            }
            _sourceTransaction.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
