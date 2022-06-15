using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Settings
{
    public class MongoDbSettings
    {
        public string Host { get; init; }
        public int Port { get; init; }

        public string User { get; init; }

        public string Password { get; init; }

        public string ConnectionString => $"mongodb://{User}:{Password}@{Host}:{Port}";
    }
}