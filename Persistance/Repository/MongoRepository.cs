using Application.MongoDB;
using Core;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Persistance.Repository
{
    public class MongoRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            _collection = database.GetCollection<T>(collectionName);
        }

        public async Task<T> GetByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<T>.Filter.Eq("_id", objectId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }
        public async Task AddManyAsync(IEnumerable<T> entities)
        {
            await _collection.InsertManyAsync(entities);
        }
        public async Task UpdateAsync(T entity)
        {
            var objectId = new ObjectId(entity.Id); // Assuming entity has an Id property
            var filter = Builders<T>.Filter.Eq("_id", objectId);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(T entity)
        {
            var objectId = new ObjectId(entity.Id); // Assuming entity has an Id property
            var filter = Builders<T>.Filter.Eq("_id", objectId);
            await _collection.DeleteOneAsync(filter);
        }
    }
}
