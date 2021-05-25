using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShortenerDataAccessLayer.Models;
using URLShortenerDomainLayer.Interfaces;
using URLShortenerDomainLayer.Models;

namespace URLShortenerDataAccessLayer
{
    public class GetUrlQuery : IQuery<GetURLByShortURLQueryParam, URLDomainModel>
    {
        public MongoClient MongoClient { get; }
        public GetUrlQuery(MongoClient mongoClient)
        {
            MongoClient = mongoClient
                ?? throw new ArgumentNullException(nameof(mongoClient));
        }
        public URLDomainModel Execute(GetURLByShortURLQueryParam param)
        {
            var filter = Builders<UrlMongoModel>.Filter.Eq("ShortUrl", param.ShortURL);
            var valueFromDb = MongoClient
                .GetDatabase("urlDatabase")
                .GetCollection<UrlMongoModel>("urls")
                .Find(filter)
                .FirstOrDefault();
            return valueFromDb?.GetDomainModel();
        }
    }
}
