﻿using System;
using System.Threading;

namespace Platform.Helpers.Threading
{
    /// <summary>
    /// TODO: Сравнить что производительнее использовать анонимную функцию или using (создание объекта + dispose)
    /// </summary>
    public class SafeSynchronization : ISyncronization
    {
#if DEBUG
        private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
#else
        private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
#endif

        public void ExecuteReadOperation(Action action)
        {
            _rwLock.EnterReadLock();
            try
            {
                action();
            }
#if DEBUG
            catch (Exception ex)
            {
                // TODO: Log exception

                throw ex;
            }
#endif
            finally
            {
                _rwLock.ExitReadLock();
            }
        }

        public T ExecuteReadOperation<T>(Func<T> func)
        {
            _rwLock.EnterReadLock();
            try
            {
                return func();
            }
#if DEBUG
            catch (Exception ex)
            {
                // TODO: Log exception

                throw ex;
            }
#endif
            finally
            {
                _rwLock.ExitReadLock();
            }
        }

        public void ExecuteWriteOperation(Action action)
        {
            _rwLock.EnterWriteLock();
            try
            {
                action();
            }
#if DEBUG
            catch (Exception ex)
            {
                // TODO: Log exception

                throw ex;
            }
#endif
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public T ExecuteWriteOperation<T>(Func<T> func)
        {
            _rwLock.EnterWriteLock();
            try
            {
                return func();
            }
#if DEBUG
            catch (Exception ex)
            {
                // TODO: Log exception

                throw ex;
            }
#endif
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }
    }
}