using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShortenerDomainLayer.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace URLShortenerDataAccessLayer.Models
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

        public UrlMongoModel(URLDomainModel domainModel)
        {
            ShortUrl = domainModel.ShortUrl;
            LongUrl = domainModel.LongUrl;
            ExpireDateTime = domainModel.ExpireDateTime;
        }

        public URLDomainModel GetDomainModel()
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
