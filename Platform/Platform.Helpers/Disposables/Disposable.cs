﻿using System;

namespace Platform.Helpers.Disposables
{
    public delegate void DisposedDelegate(bool manual);

    /// <example><code source="DisposableUsageExample.cs" /></example>
    public class Disposable : DisposableBase
    {
        private static readonly DisposedDelegate EmptyDelegate = manual => { };

        public event DisposedDelegate OnDispose = EmptyDelegate;

        public Disposable()
        {
        }

        public Disposable(Action disposed)
        {
            OnDispose = m => disposed();
        }

        public Disposable(DisposedDelegate disposed)
        {
            OnDispose = disposed;
        }

        protected override void DisposeCore(bool manual) => OnDispose(manual);

        #region Helpers

        public static bool TryDispose<T>(ref T @object)
        {
            try
            {
                var disposal = @object as DisposableBase;
                if (disposal != null)
                {
                    if (!disposal.IsDisposed)
                    {
                        disposal.Dispose();
                        @object = default(T);
                        return true;
                    }
                }
                else
                {
                    var disposable = @object as System.IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                        @object = default(T);
                        return true;
                    }
                }

            }
            catch (Exception exception)
            {
                Global.OnIgnoredException(exception);
            }

            return false;
        }

        public static bool TryDispose<T>(T @object)
        {
            return TryDispose(ref @object);
        }

        public static void DisposeIfDisposable<T>(T @object)
        {
            TryDispose(ref @object);
        }

        #endregion
    }

    public class Disposable<T> : Disposable
    {
        public readonly T Object;

        public Disposable(T @object)
        {
            Object = @object;
        }

        public Disposable(T @object, Action disposed)
            : base(disposed)
        {
            Object = @object;
        }

        public Disposable(T @object, Action<T> disposed)
        {
            Object = @object;
            OnDispose += manual => disposed(Object);
        }

        public Disposable(T @object, DisposedDelegate disposed)
            : base(disposed)
        {
            Object = @object;
        }

        public static implicit operator Disposable<T>(T @object) => new Disposable<T>(@object);

        public static implicit operator T(Disposable<T> disposable) => disposable.Object;

        protected override void DisposeCore(bool manual)
        {
            base.DisposeCore(manual);

            TryDispose(Object);
        }

        #region Helpers

        public static Disposable<T> Create(T value, Action<T> dispose)
        {
            return new Disposable<T>(value, dispose);
        }

        #endregion
    }

    public class Disposable<TPrimary, TAuxiliary> : Disposable<TPrimary>
    {
        protected readonly TAuxiliary AuxiliaryObject;

        public Disposable(TPrimary @object)
            : base(@object)
        {
        }

        public Disposable(TPrimary @object, TAuxiliary auxiliaryObject)
            : this(@object)
        {
            AuxiliaryObject = auxiliaryObject;
        }

        public static implicit operator TPrimary(Disposable<TPrimary, TAuxiliary> autoDisposable) => autoDisposable.Object;

        public static implicit operator TAuxiliary(Disposable<TPrimary, TAuxiliary> autoDisposable) => autoDisposable.AuxiliaryObject;

        protected override void DisposeCore(bool manual)
        {
            base.DisposeCore(manual);

            TryDispose(AuxiliaryObject);
        }
    }
}