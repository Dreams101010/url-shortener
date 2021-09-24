using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortenerDomainLayer.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UrlShortenerDataAccessLayer.Models
{
    public class UrlMongoModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string ShortUrl { get; set; }
        public string LongUrl { get; set; }
        public DateTime ExpireDateTime { get; set; }

        public UrlMongoModel()
        {

        }

        public UrlMongoModel(UrlDomainModel domainModel)
        {
            ShortUrl = domainModel.ShortUrl;
            LongUrl = domainModel.LongUrl;
            ExpireDateTime = domainModel.ExpireDateTime;
        }

        public UrlDomainModel GetDomainModel()
        {
            return new()
            {
                ShortUrl = ShortUrl,
                LongUrl = LongUrl,
                ExpireDateTime = ExpireDateTime,
            };
        }
    }
}
