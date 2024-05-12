using Application.MongoDB.Log;
using Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repository.Log
{
    public class LogRepository : MongoRepository<LogEntity>,ILogRepository
    {
        public LogRepository(IMongoDatabase database, string collectionName) : base(database, collectionName)
        {
        }
    }
}
