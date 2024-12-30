using BirdsCommon;
using BirdsCommon.Repository;

namespace BirdsViewModels
{
    public class BirdsViewModel : ViewModelBase
    {
        #region [ Fields ]
        private readonly IRepository<Bird> birdRepository;
        #endregion

        public BirdsViewModel(IRepository<Bird> birdRepository)
        {
            this.birdRepository = birdRepository;
            //Birds = birdRepository.GetObservableCollection();
        }

        public async Task LoadAsync() => await birdRepository.LoadAsync();


        #region [ Properties ]
        public DateOnly Departure { get; } = DateOnly.FromDateTime(DateTime.Now);

        /// <summary>Какое-то непонятное свойство. Для чего оно?</summary>
        public int NumberOfBirds { get => Get<int>(); set => Set(value); }
        #endregion
    }
}