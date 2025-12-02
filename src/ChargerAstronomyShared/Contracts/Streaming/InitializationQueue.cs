using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Contracts.Streaming
{

    /// <summary>
    /// A thread-safe blocking queue interface for managing a collection of items with blocking enqueue and dequeue operations.
    /// </summary>
    /// <typeparam name="T">The objects to be passed through the queue.</typeparam>
    public interface BlockingQueue<T>
    {

        /// <summary>
        /// The collection of items to be queued.
        /// </summary>
        IProducerConsumerCollection<T> Collection { get; }

        /// <summary>
        /// The total amount of items that can be held in the queue.
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// The current amount of items that are in the queue.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Attempts to add the item to the queue.
        /// </summary>
        /// <remarks>Note this method does not throw an exception if the operation fails. The caller can use
        /// the return value to determine whether the item was enqueued successfully.</remarks>
        /// <param name="item">The item to add to the queue.</param>
        /// <returns><see langword="true"/> if the item was successfully added to the queue; otherwise, <see langword="false"/>.</returns>
        bool TryEnqueue(T item);

        /// <summary>
        /// Attempts to remove and return the item at the beginning of the queue.
        /// </summary>
        /// <param name="item">When this method returns, contains the object removed from the queue.</param>
        /// <returns><see langword="true"/> if an object was successfully removed from the queue; otherwise, <see
        /// langword="false"/>.</returns>
        bool TryDequeue(out T item);

        /// <summary>
        /// Adds an item to the queue, blocking if the queue is full until space becomes available or the operation is
        /// canceled.
        /// </summary>
        /// <remarks>This method blocks the calling thread if the queue is full, waiting until space
        /// becomes available. Ensure that the <paramref name="ct"/> token is monitored to avoid indefinite blocking in
        /// scenarios if cancellation is required.</remarks>
        /// <param name="item">The item to add to the queue.</param>
        /// <param name="ct">A <see cref="System.Threading.CancellationToken"/> that can be used to cancel the operation.</param>
        void EnqueueBlocking(T item, System.Threading.CancellationToken ct);

        /// <summary>
        /// Marks that the producer will not add more items to the queue. There may still be items left to dequeue.
        /// </summary>
        void Complete();

        /// <summary>
        /// Marks whether the queue has been marked complete and is fully drained.
        /// </summary>
        bool IsCompleted { get; }
    }

}