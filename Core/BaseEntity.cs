using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [BsonElement("DeletedAt")]
        public DateTime? DeletedAt { get; set; }
        [BsonElement("IsDeleted")]
        public bool IsDeleted { get; set; } = false;
    }
}
