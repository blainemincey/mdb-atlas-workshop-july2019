using System;
using MongoDB.Bson;
using MongoDB.Driver;

// Be sure to add the MongoDB .NET Driver!
// dotnet add package MongoDB.Driver --version 2.8.1

namespace ConsoleApp1
{
    class Program
    {
        private static string MONGO_URL = "### YOUR MONGODB ATLAS URL ###";
        
        static void Main(string[] args)
        {
            Console.WriteLine("Starting MyMongoDBApp: " + DateTime.Now);

            var client = new MongoClient(MONGO_URL);
            var database = client.GetDatabase("sample_mflix");
            var collection = database.GetCollection<BsonDocument>("movies");
            
            // Our filter
            var filter =
                new BsonDocument
                {
                    {"year", new BsonDocument("$gte", 2015)},
                    {"cast", "Chris Pratt"}
                };
            
            // Our projection
            var projection = "{_id: 0, title: 1, cast: 1}";
            
            // If you are passing an empty filter, limit the returned docs to 100
            var cursor = collection.Find(new BsonDocument(filter)).Project(projection).Limit(100).ToCursor();
            
            foreach (var document in cursor.ToEnumerable())
            {
                Console.WriteLine(document);   
            }
            
            Console.WriteLine("Ending MyMongoDBApp: " + DateTime.Now);
        }
    }
}