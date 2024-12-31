using BirdsCommon;
using BirdsCommon.Repository;

namespace BirdsViewModels
{
#pragma warning disable CS9113 // Параметр не прочитан.
    public class AddBirdViewModel(IRepository<Bird> birdRepository) : ViewModelBase
#pragma warning restore CS9113 // Параметр не прочитан.
    {

    }
}
