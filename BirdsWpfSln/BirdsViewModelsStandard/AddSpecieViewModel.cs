using BirdsCommonStandard;

namespace BirdsViewModels
{
    public class AddSpecieViewModel : ViewModelBase
    {
        public AddSpecieViewModel(ViewModelSettings settings)
            : base(settings)
        { }

        public string Name { get => Get<string>(); set => Set(value); }
    }
}