using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Infrastructure.Repositories
{
    public class PromocionRepository
    {
        public MongoClient client;
        public IMongoDatabase db;
        public PromocionRepository()
        {
            client = new MongoClient("mongodb://127.0.0.1:27017");
            db = client.GetDatabase("Inventory");

        }
    }
}
