namespace BirdsCommonStandard
{
    public interface IBirdsModel
    {
        IRepository<Bird> Birds { get; }
        IRepository<Specie> Species { get; }
    }
}
