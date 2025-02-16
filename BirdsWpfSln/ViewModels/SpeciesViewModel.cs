using BirdsCommon.Repository;
using BirdsCommon.ViewModelBase;

namespace BirdsViewModels
{
    public class SpeciesViewModel : ViewModelBase
    {
        private readonly IRepository<Specie> speciesRepository;

        public SpeciesViewModel(IRepository<Specie> speciesRepository)
        {
            this.speciesRepository = speciesRepository;
        }
    }
}