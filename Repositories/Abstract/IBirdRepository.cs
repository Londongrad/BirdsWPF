using BirdsWPF.Models;

namespace BirdsWPF.Repositories.Abstract
{
    public interface IBirdRepository
    {
        Task AddAsync(BirdEntity bird);
        Task DeleteAsync(int id);
        Task<IEnumerable<BirdEntity>> GetAllAsync();
        Task UpdateAsync(BirdEntity bird);
    }
}
