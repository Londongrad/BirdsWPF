namespace BirdsCommon.Repository
{
    /// <summary>Имутабельный класс для Species (Вид). </summary>
    public class Specie(int id, string name)
        : IdDto(id)
    {
        /// <summary>Название разновидности.</summary>
        public string Name { get; set; } = name;
    }
}
