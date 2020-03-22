using System;
using System.Threading;
using UniRx;

namespace TU.Unity.World
{
    public static class RxExtensions
    {
        /// <summary>
        ///     Awaiter for TimeSpan.
        /// </summary>
        /// <param name="timeSpan">Any TimeSpan</param>
        /// <returns>Awaiter</returns>
        public static AsyncSubject<long> GetAwaiter(this TimeSpan timeSpan) =>
            Observable.Timer(timeSpan).GetAwaiter();

        /// <summary>
        ///     Awaiter for TimeSpan with Cancellation.
        /// </summary>
        /// <param name="timeSpan">Any TimeSpan</param>
        /// <returns>Awaiter</returns>
        public static AsyncSubject<long> GetAwaiter(this TimeSpan timeSpan, CancellationToken cancellationToken) =>
            Observable.Timer(timeSpan).GetAwaiter(cancellationToken);
    }
}