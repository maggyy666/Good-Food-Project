using GoodFoodProjectMVC.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodFoodProjectMVC.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<FormData> _formDataCollection;
        private readonly IMongoCollection<User> _userCollection;

        public MongoDBService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetSection("MongoDB:ConnectionString").Value);
            var database = client.GetDatabase(configuration.GetSection("MongoDB:DatabaseName").Value);
            _formDataCollection = database.GetCollection<FormData>("Recipes");
            _userCollection = database.GetCollection<User>("Users");
        }

        // Recipe Methods
        public async Task<List<FormData>> GetAllRecipesAsync()
        {
            return await _formDataCollection.Find(Builders<FormData>.Filter.Empty).ToListAsync();
        }

        public async Task<FormData> GetRecipeByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            return await _formDataCollection.Find(Builders<FormData>.Filter.Eq("_id", objectId)).FirstOrDefaultAsync();
        }

        public async Task InsertDataAsync(FormData formData)
        {
            await _formDataCollection.InsertOneAsync(formData);
        }

        public async Task UpdateRecipeAsync(FormData updatedRecipe)
        {
            var filter = Builders<FormData>.Filter.Eq("_id", new ObjectId(updatedRecipe.Id));
            var update = Builders<FormData>.Update
                .Set(r => r.Title, updatedRecipe.Title)
                .Set(r => r.Description, updatedRecipe.Description)
                .Set(r => r.ImageBase64, updatedRecipe.ImageBase64);

            await _formDataCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteRecipeByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<FormData>.Filter.Eq("_id", objectId);
            await _formDataCollection.DeleteOneAsync(filter);
        }

        // User Methods
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userCollection.Find(Builders<User>.Filter.Empty).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            return await _userCollection.Find(Builders<User>.Filter.Eq("_id", objectId)).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUsernameAndPasswordAsync(string username, string password)
        {
            return await _userCollection.Find(user => user.Username == username && user.Password == password).FirstOrDefaultAsync();
        }

        public async Task InsertUserAsync(User user)
        {
            await _userCollection.InsertOneAsync(user);
        }
    }
}
