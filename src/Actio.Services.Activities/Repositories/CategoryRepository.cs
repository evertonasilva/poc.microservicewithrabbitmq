using System.Collections.Generic;
using System.Threading.Tasks;
using Actio.Services.Activities.Domain.Models;
using Actio.Services.Activities.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Actio.Services.Activities.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoDatabase _mongoDatabase;

        public CategoryRepository(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase=mongoDatabase;
        }

        public async Task AddAsync(Category category)  => await Collection.InsertOneAsync(category);
        public async Task<IEnumerable<Category>> BrowseAsync()=> await Collection.AsQueryable().ToListAsync();
        public async Task<Category> GetAsync(string name)=> await Collection.AsQueryable().FirstOrDefaultAsync(x=>x.Name == name);
        private IMongoCollection<Category> Collection => _mongoDatabase.GetCollection<Category>("Categories");
    }
}