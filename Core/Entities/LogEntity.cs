using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class LogEntity:BaseEntity
    {
        [BsonElement("LogData")]
        public string LogData { get; set; }
    }
}
