using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Link
    {
        public Link(string id, string shortenedLink)
        {
            _id = id;
            ShortenedLink = shortenedLink;
            LastAccessed = DateTime.Now;
        }

        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string _id { get; set; }

        public string ShortenedLink { get; set; }
        public DateTime LastAccessed { get; set; }
    }
}