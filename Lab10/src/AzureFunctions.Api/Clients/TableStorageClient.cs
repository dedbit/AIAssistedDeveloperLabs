using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureFunctions.Api.Clients
{
    public class TableStorageClient
    {
        private readonly CloudStorageAccount _storageAccount;
        private readonly string _storageConnectionString;
        private readonly CloudTableClient _tableClient;
        private readonly string _tableName;

        public TableStorageClient(string tableStorageConnectionString, string tableName)
        {
            _storageConnectionString = tableStorageConnectionString;
            _tableName = tableName;

            _storageAccount = CloudStorageAccount.Parse(_storageConnectionString);
            _tableClient = _storageAccount.CreateCloudTableClient();
            Table = _tableClient.GetTableReference(_tableName);
        }

        public CloudTable Table { get; }

        public async Task<TableResult> Insert(ITableEntity entity)
        {
            await Table.CreateIfNotExistsAsync();

            TableOperation operation = TableOperation.Insert(entity);
            TableResult result = await Table.ExecuteAsync(operation);
            return result;
        }

        public async Task<TableResult> InsertOrMerge(ITableEntity entity)
        {
            await Table.CreateIfNotExistsAsync();

            TableOperation operation = TableOperation.InsertOrMerge(entity);
            TableResult result = await Table.ExecuteAsync(operation);
            return result;
        }

        public async Task<TableResult> MergeChange(ITableEntity entity)
        {
            await Table.CreateIfNotExistsAsync();

            TableOperation operation = TableOperation.Merge(entity);
            TableResult result = await Table.ExecuteAsync(operation);
            return result;
        }

        public async Task<TableResult> Delete(ITableEntity entity)
        {
            TableOperation operation = TableOperation.Delete(entity);

            TableResult result = await Table.ExecuteAsync(operation);

            return result;
        }

        public async Task<DynamicTableEntity> Retrieve(string partitionKey, string rowKey)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve(partitionKey, rowKey);
            TableResult result = await Table.ExecuteAsync(retrieveOperation);
            if (result == null) return null;
            return result.Result as DynamicTableEntity;
        }

        public async Task<T> Retrieve<T>(string partitionKey, string rowKey) where T : TableEntity
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            TableResult retrievedResult = await Table.ExecuteAsync(retrieveOperation);

            return retrievedResult.Result as T;
        }

        /// <summary>
        /// This method queries items from table storage. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filterString">I.e. InternalTransactionId eq 'ef5140d7-cf6d-49db-9005-57bcaf6a4b99'</param>
        /// <param name="selectColumns">If set, only the columns selected will be returned</param>
        /// <param name="takeCount"></param>
        /// <returns>A List of items matching the query</returns>
        public List<T> Query<T>(string filterString = "", IList<string> selectColumns = null,
            int? takeCount = null) where T : ITableEntity, new()
        {
            TableQuery<T> query = new TableQuery<T>();
            query.FilterString = filterString;
            if (selectColumns != null)
                query.SelectColumns = selectColumns;

            if (takeCount != null)
                query.TakeCount = takeCount;

            TableContinuationToken continuationToken = null;
            List<T> transactionStatusList = new List<T>();

            do
            {
                var queryResult = Table.ExecuteQuerySegmentedAsync(query, continuationToken);
                continuationToken = queryResult.Result.ContinuationToken;
                transactionStatusList.AddRange(queryResult.Result.Results.ToList());

            } while (continuationToken != null);

            return transactionStatusList;
        }
    }
}
