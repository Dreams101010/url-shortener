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
    public class AddUrlCommand : ICommand<AddUrlCommandParam, bool>
    {
        public MongoClient MongoClient { get; }
        public AddUrlCommand(MongoClient mongoClient)
        {
            MongoClient = mongoClient 
                ?? throw new ArgumentNullException(nameof(mongoClient));
        }

        public bool Execute(AddUrlCommandParam param)
        {
            var model = new UrlMongoModel(param.UrlToAdd);
            MongoClient.GetDatabase("urlDatabase")
                .GetCollection<UrlMongoModel>("urls")
                .InsertOne(model);
            return true;
        }
    }
}
