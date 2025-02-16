using BirdsCommon.ViewModelBase;

namespace BirdsViewModels
{
    public class AddSpecieViewModel : ViewModelBase
    {
        /// <summary>Имя.</summary>
        public string? Name { get => Get<string?>(); set => Set(value); }

    }
}
