﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LoggingClientSocket.cs -- logging socket for debug
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.IO;
using AM.Logging;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Infrastructure.Sockets
{
    /// <summary>
    /// Logging socket for debug purposes.
    /// </summary>
    [PublicAPI]
    public sealed class LoggingClientSocket
        : AbstractClientSocket
    {
        #region Properties

        /// <summary>
        /// Path to store debug data.
        /// </summary>
        [NotNull]
        public string DebugPath { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public LoggingClientSocket
            (
                [NotNull] IrbisConnection connection,
                [NotNull] AbstractClientSocket innerSocket,
                [NotNull] string debugPath
            )
            : base(connection)
        {
            Sure.NotNull(innerSocket, nameof(innerSocket));
            Sure.NotNullNorEmpty(debugPath, nameof(debugPath));

            if (!Directory.Exists(debugPath))
            {
                throw new IrbisNetworkException
                    (
                        "directory not exist: " + debugPath
                    );
            }

            DebugPath = debugPath;
            InnerSocket = innerSocket;
        }

        #endregion

        #region Private members

        private static int _counter;

        private void _DumpGeneralInfo
            (
                string suffix,
                string text
            )
        {
            int counter = Interlocked.Increment(ref _counter);

            string path = Path.Combine
                (
                    DebugPath,
                    string.Format
                    (
                        "{0:00000000}{1}.packet",
                        counter,
                        suffix
                    )
                );
            File.WriteAllText
                (
                    path,
                    text
                );
        }

        private void _DumpException
            (
                Exception exception
            )
        {
            _DumpGeneralInfo
                (
                    "ex",
                    exception.ToString()
                );
        }

        private void _DumpPackets
            (
                byte[] request,
                byte[] answer
            )
        {
            int counter = Interlocked.Increment(ref _counter);

            string upPath = Path.Combine
                (
                    DebugPath,
                    string.Format
                    (
                        "{0:00000000}up.packet",
                        counter
                    )
                );
            File.WriteAllBytes(upPath, request);

            string downPath = Path.Combine
                (
                    DebugPath,
                    string.Format
                    (
                        "{0:00000000}dn.packet",
                        counter
                    )
                );
            File.WriteAllBytes(downPath, answer);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Set start value for packet counter.
        /// </summary>
        public static void SetCounter
            (
                int startValue
            )
        {
            Sure.NonNegative(startValue, nameof(startValue));

            _counter = startValue;
        }

        /// <summary>
        /// Do some setup before using.
        /// </summary>
        public static void Setup
            (
                [NotNull] string debugPath,
                bool clearDirectory
            )
        {
            Sure.NotNullNorEmpty(debugPath, "debugPath");

            if (!Directory.Exists(debugPath))
            {
                Directory.CreateDirectory(debugPath);
            }

            if (clearDirectory)
            {
                DirectoryUtility.ClearDirectory(debugPath);
            }
            else
            {
                _counter = Directory.GetFiles(debugPath).Length;
            }
        }

        #endregion

        #region AbstractClientSocket members

        /// <summary>
        /// Abort the request.
        /// </summary>
        public override void AbortRequest()
        {
            AbstractClientSocket innerSocket = InnerSocket
                .ThrowIfNull("InnerSocket");

            innerSocket.AbortRequest();
        }

        /// <summary>
        /// Send request to server and receive answer.
        /// </summary>
        public override byte[] ExecuteRequest
            (
                byte[] request
            )
        {
            Sure.NotNull(request, "request");

            AbstractClientSocket innerSocket = InnerSocket
                .ThrowIfNull("InnerSocket");

            byte[] result;
            try
            {
                result = innerSocket.ExecuteRequest(request);
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "LoggingClientSocket::ExecuteRequest",
                        exception
                    );

                Task.Factory.StartNew
                    (
                        () => _DumpException(exception)
                    );
                throw;
            }

            Task.Factory.StartNew
                (
                    () => _DumpPackets(request, result)
                );

            return result;
        }

        #endregion
    }
}

