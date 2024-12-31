namespace BirdsCommon.Repository
{
    /// <summary>Базовый класс с идентификатором.</summary>
    public record IdDto
    {

        /// <summary>Целочисленный идентификатор.</summary>
        public int Id { get; set; }

    }
}
