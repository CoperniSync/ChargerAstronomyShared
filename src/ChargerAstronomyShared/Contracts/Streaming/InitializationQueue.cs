using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Contracts.Streaming
{
    public interface IInitializationQueue<T>
    {
        IProducerConsumerCollection<T> Collection { get; }

        int Capacity { get; }

        int Count { get; }

        bool TryEnqueue(T item);

        bool TryDequeue(out T item);

        void EnqueueBlocking(T item, System.Threading.CancellationToken ct); 

        void Complete();

        bool IsCompleted { get; }
    }

}