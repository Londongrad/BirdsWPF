using BirdsWPF.Data;
using BirdsWPF.Models;
using BirdsWPF.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BirdsWPF.Repositories.Bases
{
    public class BirdRepository(ApplicationDbContext context) : IBirdRepository
    {
        #region [ Methods ]
        public async Task AddAsync(BirdEntity bird)
        {
            await context.Birds!.AddAsync(bird);
            await context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await context.Birds!
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();
        }
        public async Task<IEnumerable<BirdEntity>> GetAllAsync()
        {
            return await context.Birds!.AsNoTracking().ToListAsync();
        }
        public async Task UpdateAsync(BirdEntity bird)
        {
            await context.Birds!
                .Where(c => c.Id == bird.Id)
                .ExecuteUpdateAsync(sp => sp
                    .SetProperty(c => c.Name, bird.Name)
                    .SetProperty(c => c.Arrival, bird.Arrival)
                    .SetProperty(c => c.Description, bird.Description)
                    .SetProperty(c => c.Departure, bird.Departure)
                    .SetProperty(c => c.IsActive, bird.IsActive));
        }
        #endregion
    }
}
