using Microsoft.Azure.WebJobs;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AzureFunctions.Api.Tests.Mocks
{
    /// <summary>
    /// Can be used with an Azure queue output binding. 
    /// I.e.: [Queue("userupdates")] IAsyncCollector<string> outputSbQueue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncCollectorMock<T> : IAsyncCollector<T>
    {
        public readonly List<T> Items = new List<T>();

        public Task AddAsync(T item, CancellationToken cancellationToken = default(CancellationToken))
        {

            Items.Add(item);

            return Task.FromResult(true);
        }

        public Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(true);
        }
    }
}