using BirdsCommon.Repository;

namespace BirdsRepository
{
    public partial class BirdsDbContext
    {
        /// <summary>Класс с демо-данными для первоначальной инициализации новой БД.</summary>
        private class HasData
        {
            private static readonly Specie[] species = "Воробьи Вороны Ястребы".Split().Select((s, i) => new Specie(i + 1, s)).ToArray();
            private static readonly Bird[] birds =
@"Воробей 1
Дрозд 1
Скворец 1
Снегирь 1
Ворон 2
Сорока 2
Галка 2
Ястреб 3
Орёл 3"
                .Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Split())
                .Select((s, i) => new Bird(i + 1, s[0], "описание для " + s[0], new DateOnly(), new DateOnly(), true, int.Parse(s[1])))
                .ToArray();
            public static IEnumerable<Specie> GetSpecies() => species.Select(x => x);
            public static IEnumerable<Bird> GetBirds() => birds.Select(x => x);
        }

    }
}
