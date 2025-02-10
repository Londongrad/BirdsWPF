using BirdsCommonStandard;

namespace BirdsViewModels
{
    public class EditSpecieViewModel : ViewModelBase
    {
        public EditSpecieViewModel(ViewModelSettings settings)
            : base(settings)
        { }

        public int SpecieId { get => Get<int>(); set => Set(value); }
        public string Name { get => Get<string>(); set => Set(value); }
    }
}