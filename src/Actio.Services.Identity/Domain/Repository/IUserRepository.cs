using System;
using System.Threading.Tasks;
using Actio.Services.Identity.Domain.Models;

namespace Actio.Services.Identity.Domain.Repository
{
    public interface IUserRepository
    {
         Task<User> GetAsync(Guid id); 
         Task<User> GetAsync(string email);
         Task AddAsync(User user);
    }
}