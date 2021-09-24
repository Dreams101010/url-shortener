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
    public class GetUrlQuery : IQuery<GetUrlByShortUrlQueryParam, UrlDomainModel>
    {
        public MongoClient MongoClient { get; }
        public GetUrlQuery(MongoClient mongoClient)
        {
            MongoClient = mongoClient
                ?? throw new ArgumentNullException(nameof(mongoClient));
        }
        public UrlDomainModel Execute(GetUrlByShortUrlQueryParam param)
        {
            var filter = Builders<UrlMongoModel>.Filter.Eq("ShortUrl", param.ShortUrl);
            var valueFromDb = MongoClient
                .GetDatabase("urlDatabase")
                .GetCollection<UrlMongoModel>("urls")
                .Find(filter)
                .FirstOrDefault();
            return valueFromDb?.GetDomainModel();
        }
    }
}
