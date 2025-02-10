namespace BirdsCommonStandard
{
    /// <summary>Базовый класс с идентификатором.</summary>
    public class IdDto
    {
        /// <summary>Целочисленный идентификатор.</summary>
        public int Id { get; } 

        public IdDto(int id)
        {
            Id = id;
        }
    }
}
