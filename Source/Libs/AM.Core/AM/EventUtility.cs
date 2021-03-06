﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EventUtility.cs -- Useful routines for event manipulations
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// Useful routines for event manipulations.
    /// </summary>
    [PublicAPI]
    public static class EventUtility
    {
        #region Delegates

        /// <summary>
        /// ???
        /// </summary>
        public delegate void RemoveDelegate(EventHandler handler);

        #endregion

        #region Public methods

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static void Raise
            (
                [CanBeNull] this EventHandler handler,
                [CanBeNull] object sender,
                [CanBeNull] EventArgs args
            )
        {
            handler?.Invoke(sender, args);
        }

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static void Raise<T>
            (
                [CanBeNull] this EventHandler<T> handler,
                [CanBeNull] object sender,
                [CanBeNull] T args
            )
            where T : EventArgs
        {
            handler?.Invoke(sender, args);
        }

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static void Raise<T>
            (
                [CanBeNull] this EventHandler<T> handler,
                [CanBeNull] object sender
            )
            where T : EventArgs
        {
            handler?.Invoke(sender, null);
        }

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static void Raise
            (
                [CanBeNull] this EventHandler handler,
                [CanBeNull] object sender
            )
        {
            handler?.Invoke(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static void Raise<T>
            (
                [CanBeNull] this EventHandler<T> handler
            )
            where T : EventArgs
        {
            handler?.Invoke(null, null);
        }

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static Task RaiseAsync
            (
                [CanBeNull] this EventHandler handler,
                [CanBeNull] object sender,
                [CanBeNull] EventArgs args
            )
        {
            Task result = Task.Factory.StartNew
                (
                    () =>
                    {
                        handler?.Invoke(sender, args);
                    }
                );

            return result;
        }

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static Task RaiseAsync
            (
                [CanBeNull] this EventHandler handler,
                [CanBeNull] object sender
            )
        {
            Task result = Task.Factory.StartNew
                (
                    () =>
                    {
                        handler?.Invoke(sender, EventArgs.Empty);
                    }
                );

            return result;
        }

        #endregion
    }
}

