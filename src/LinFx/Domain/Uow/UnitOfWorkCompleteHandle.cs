using System;
using System.Threading.Tasks;

namespace LinFx.Domain.Uow
{
    public interface IUnitOfWorkCompleteHandle : IDisposable
    {
        /// <summary>
        /// Completes this unit of work.
        /// It saves all changes and commit transaction if exists.
        /// </summary>
        void Complete();
		/// <summary>
		/// Completes this unit of work.
		/// It saves all changes and commit transaction if exists.
		/// </summary>
		Task CompleteAsync();
	}

    public class InnerUnitOfWorkCompleteHandle : IUnitOfWorkCompleteHandle
    {
        public const string DidNotCallCompleteMethodExceptionMessage = "Did not call Complete method of a unit of work.";

        //private volatile bool _isCompleteCalled = false;
        private volatile bool _isDisposed;

        public void Complete()
        {
            //_isCompleteCalled = true;
        }

		public Task CompleteAsync()
		{
			throw new NotImplementedException();
		}

		public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            //if (!_isCompleteCalled)
            //{
            //    if (HasException())
            //    {
            //        return;
            //    }

            //    throw new Exception(DidNotCallCompleteMethodExceptionMessage);
            //}
        }

        //private static bool HasException()
        //{
        //    try
        //    {
        //        return Marshal.GetExceptionCode() != 0;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
    }
}
