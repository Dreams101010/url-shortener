using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortenerDataAccessLayer.Models;
using UrlShortenerDomainLayer.Interfaces;
using UrlShortenerDomainLayer.Models;

namespace UrlShortenerDataAccessLayer
{
    public class RemoveUrlByShortUrlCommand : ICommand<RemoveUrlByShortUrlCommandParam, bool>
    {
        public MongoClient MongoClient { get; }
        public RemoveUrlByShortUrlCommand(MongoClient mongoClient)
        {
            MongoClient = mongoClient
                ?? throw new ArgumentNullException(nameof(mongoClient));
        }

        public bool Execute(RemoveUrlByShortUrlCommandParam param)
        {
            var filter = Builders<UrlMongoModel>.Filter.Eq("ShortUrl", param.ShortUrl);
            var valueFromDb = MongoClient
                .GetDatabase("urlDatabase")
                .GetCollection<UrlMongoModel>("urls")
                .DeleteOne(filter);
            return valueFromDb.DeletedCount > 0;
        }
    }
}
