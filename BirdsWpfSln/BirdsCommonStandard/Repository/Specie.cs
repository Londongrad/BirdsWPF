namespace BirdsCommonStandard
{
    /// <summary>Имутабельный класс для Species (Вид). </summary>
    public class Specie : IdDto
    {
        public Specie(int id, string name) : base(id) => Name = name ?? string.Empty;

        /// <summary>Название разновидности.</summary>
        public string Name { get; }
    }
}
