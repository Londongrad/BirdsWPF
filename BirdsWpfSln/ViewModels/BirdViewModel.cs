using BirdsCommon.Repository;
using BirdsCommon.ViewModelBase;

namespace BirdsViewModels
{
    public class BirdViewModel : ViewModelBase
    {
        #region [ Fields ]
        private readonly IRepository<Bird> birdRepository;
        #endregion

        public BirdViewModel(IRepository<Bird> birdRepository)
        {
            this.birdRepository = birdRepository;
            //Birds = birdRepository.GetObservableCollection();
        }

        public async Task LoadAsync() => await birdRepository.LoadAsync();


        #region [ Properties ]
        #endregion
    }
}