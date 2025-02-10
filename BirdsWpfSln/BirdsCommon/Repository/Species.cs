namespace BirdsCommon.Repository
{
    /// <summary>Имутабельный класс для Species (Вид). </summary>
    public class Species(int id, string name) : IdDto(id)
    {
        /// <summary>Название разновидности.</summary>
        public string Name { get; /*set;*/ } = name;

        ///// <summary>Птицы, относящиеся к разновидности.</summary>
        //public List<Bird> Birds { get; set; } = birds;
    }
}
