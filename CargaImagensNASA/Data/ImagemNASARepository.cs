using System;
using MongoDB.Driver;
using CargaImagensNASA.Documents;

namespace CargaImagensNASA.Data
{
    public static class ImagemNASARepository
    {
        public static void SaveInfoImagem(ImagemNASADocument infoImagem)
        {
            var mongoClient = new MongoClient(
                Environment.GetEnvironmentVariable("MongoConnection"));
            var mongoDatabase = mongoClient.GetDatabase(
                Environment.GetEnvironmentVariable("MongoDatabase"));
            var mongoCollection = mongoDatabase
                .GetCollection<ImagemNASADocument>(
                    Environment.GetEnvironmentVariable("MongoCollection"));
            mongoCollection.InsertOne(infoImagem);
        }
    }
}