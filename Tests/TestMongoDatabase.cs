using System;
using MongoDB.Driver;

namespace Tests
{
    public static class TestMongoDatabase
    {
        public static IMongoDatabase Create()
        {
            var mongoConnectionString = "mongodb+srv://mistral:Zz1478963@cluster0.xg2aa.mongodb.net/myFirstDatabase?retryWrites=true&w=majority";
            var mongoClient = new MongoClient(mongoConnectionString);
            return mongoClient.GetDatabase("game-tests");
        }
    }
}