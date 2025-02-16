namespace BirdsCommon.Repository
{
    public interface IBirdsModel
    {
        public IRepository<Bird> Birds { get; }
        public IRepository<Specie> Species { get; }

        Task LoadAsync();
    }
}
