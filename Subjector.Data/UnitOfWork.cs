using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Subjector.Data.Entities;
using Subjector.Data.Repository;

namespace Subjector.Data
{
    public class UnitOfWork : IDisposable
    {
        #region Fields

        /// <summary>
        /// Data context
        /// </summary>
        private DbContext context;

        private UserRepository _userRepository;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Data context
        /// </summary>
        public DbContext DataContext
        {
            get
            {
                return context ?? (context = new SubjectorContext());
            }
        }

        #region Repository


        public UserRepository UserRepository
        {
            get
            {
                return _userRepository ?? (_userRepository = new UserRepository(DataContext));
            }
        }

        #endregion Repository

        #endregion Properties

        #region Methods

        /// <summary>
        /// Save changes for unit of work async
        /// </summary>
        public async Task SaveAsync()
        {
            context.ChangeTracker.DetectChanges();
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Save changes for unit of work
        /// </summary>
        public void Save()
        {
            context.ChangeTracker.DetectChanges();
            context.SaveChanges();
        }

        #endregion Methods

        #region IDisposable Members

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (context != null)
                        context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose objects
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Members
    }
}
