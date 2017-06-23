﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using Eventus.Cleanup;
using Eventus.DocumentDb;
using Eventus.DocumentDb.Config;
using Eventus.Logging;
using Eventus.Samples.Core.Domain;
using Eventus.Storage;
using Microsoft.Azure.Documents.Client;

namespace Eventus.Samples.Infrastructure.Factories.Providers
{
    public class DocumentDbProviderFactory : ProviderFactory
    {
        private static DocumentClient _client;
        private static readonly string DatabaseId = "Eventus";
        private static readonly string Endpoint = "https://localhost:8081/";
        private static readonly string AuthKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        public DocumentDbProviderFactory(int value, string displayName) : base(value, displayName)
        {
        }

        public override Task<ITeardown> CreateTeardownAsync()
        {
            return Task.FromResult<ITeardown>(new DocumentDbTeardown(Client, DatabaseId));
        }

        public override Task<IEventStorageProvider> CreateEventStorageProviderAsync()
        {
            var provider = new DocumentDbStorageProvider(Client, DatabaseId);
            return Task.FromResult<IEventStorageProvider>(new EventStorageProviderLoggingDecorator(provider));
        }

        public override Task<ISnapshotStorageProvider> CreateSnapshotStorageProviderAsync()
        {
            var provider = new DocumentDbSnapShotProvider(Client, DatabaseId, 3);
            return Task.FromResult<ISnapshotStorageProvider>(new SnapshotProviderLoggingDecorator(provider));
        }

        public override Task InitAsync()
        {
            var init = new DocumentDbInitialiser(Client, new DocumentDbConfig(DatabaseId, typeof(BankAccount).GetTypeInfo().Assembly));
            return init.InitAsync();
        }

        private static DocumentClient Client => _client ?? (_client = new DocumentClient(new Uri(Endpoint), AuthKey, new ConnectionPolicy { EnableEndpointDiscovery = false }));
    }
}