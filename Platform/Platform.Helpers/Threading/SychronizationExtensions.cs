﻿using System;
using System.Runtime.CompilerServices;

namespace Platform.Helpers.Threading
{
    /// <remarks>
    /// TODO: Избавиться от анонимных функций передаваемых в ExecuteReadOperation и ExecureWriteOperation
    /// </remarks>
    public static class SychronizationExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult ExecuteReadOperation<TResult, TParam>(this ISynchronization synchronization, TParam parameter, Func<TParam, TResult> function)
        {
            return synchronization.ExecuteReadOperation(() => function(parameter));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ExecuteReadOperation<TParam>(this ISynchronization synchronization, TParam parameter, Action<TParam> action)
        {
            synchronization.ExecuteReadOperation(() => action(parameter));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult ExecuteWriteOperation<TResult, TParam>(this ISynchronization synchronization, TParam parameter, Func<TParam, TResult> function)
        {
            return synchronization.ExecuteWriteOperation(() => function(parameter));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ExecuteWriteOperation<TParam>(this ISynchronization synchronization, TParam parameter, Action<TParam> action)
        {
            synchronization.ExecuteWriteOperation(() => action(parameter));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult ExecuteReadOperation<TResult, TParam1, TParam2>(this ISynchronization synchronization, TParam1 parameter1, TParam2 parameter2, Func<TParam1, TParam2, TResult> function)
        {
            return synchronization.ExecuteReadOperation(() => function(parameter1, parameter2));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ExecuteReadOperation<TParam1, TParam2>(this ISynchronization synchronization, TParam1 parameter1, TParam2 parameter2, Action<TParam1, TParam2> action)
        {
            synchronization.ExecuteReadOperation(() => action(parameter1, parameter2));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult ExecuteWriteOperation<TResult, TParam1, TParam2>(this ISynchronization synchronization, TParam1 parameter1, TParam2 parameter2, Func<TParam1, TParam2, TResult> function)
        {
            return synchronization.ExecuteWriteOperation(() => function(parameter1, parameter2));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ExecuteWriteOperation<TParam1, TParam2>(this ISynchronization synchronization, TParam1 parameter1, TParam2 parameter2, Action<TParam1, TParam2> action)
        {
            synchronization.ExecuteWriteOperation(() => action(parameter1, parameter2));
        }
    }
}