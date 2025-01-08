using BirdsCommon;
using BirdsCommon.Repository;

namespace BirdsViewModels
{
    public class BirdVM : ViewModelBase
    {
        public int Id { get;}
        public Bird? Bird { get => Get<Bird>(); private set => Set(value); }

        public void SetBird(Bird bird)
        {
            if (bird is not null && bird.Id != Id)
                throw new ArgumentException("Не совпадает Id.", nameof(bird));
            Bird = bird;
        }

        public BirdVM(int id)
        {
            Id = id;
        }

        public BirdVM(Bird bird)
            : this(bird?.Id ?? throw new ArgumentNullException(nameof(bird)))
        {
            Bird = bird;
        }
    }
}
