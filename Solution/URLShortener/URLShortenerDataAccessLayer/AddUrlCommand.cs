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
    public class AddUrlCommand : ICommand<AddURLCommandParam, bool>
    {
        public MongoClient MongoClient { get; }
        public AddUrlCommand(MongoClient mongoClient)
        {
            MongoClient = mongoClient 
                ?? throw new ArgumentNullException(nameof(mongoClient));
        }

        public bool Execute(AddURLCommandParam param)
        {
            var model = new UrlMongoModel(param.URLToAdd);
            MongoClient.GetDatabase("urlDatabase")
                .GetCollection<UrlMongoModel>("urls")
                .InsertOne(model);
            return true;
        }
    }
}
