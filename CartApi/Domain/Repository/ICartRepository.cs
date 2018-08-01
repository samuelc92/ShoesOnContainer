using System.Collections.Generic;
using System.Threading.Tasks;
using CartApi.Domain.Models;

namespace CartApi.Domain.Repository
{
    public interface ICartRepository
    {
        Task<Cart> GetCartAsync(string cartId);

        IEnumerable<string> GetUsers();

        Task<Cart> UpdateCartAsync(Cart basket);

        Task<bool> DeleteCartAsync(string id);
    }
}