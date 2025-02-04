using BirdsCommon;
using BirdsCommon.Repository;

namespace BirdsViewModels
{
    public class BirdVM(int id) : ViewModelBase
    {
        public int Id { get; } = id;
        public Bird? Bird { get => Get<Bird>(); private set => Set(value); }

        public void SetBird(Bird bird)
        {
            if (bird is not null && bird.Id != Id)
                throw new ArgumentException("Не совпадает Id.", nameof(bird));
            Bird = bird;
            if (bird is null || bird.IsActive)
            {
                EditableDeparture = DateOnly.FromDateTime(DateTime.Now);
            }
            else
            {
                EditableDeparture = bird.Departure;
            }
        }

        public BirdVM(Bird bird)
            : this(bird?.Id ?? throw new ArgumentNullException(nameof(bird)))
        {
            SetBird(bird);
        }

        public DateOnly EditableDeparture { get => Get<DateOnly>(); set => Set(value); }
    }
}
