namespace BirdsCommon.Repository
{
    /// <summary>Базовый класс с идентификатором.</summary>
    public class IdDto(int id)
    {
        /// <summary>Целочисленный идентификатор.</summary>
        public int Id { get; } = id;
    }
}
