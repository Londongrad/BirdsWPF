using BirdsCommon.Repository;
using BirdsCommon.ViewModelBase;

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
        #endregion
    }
}