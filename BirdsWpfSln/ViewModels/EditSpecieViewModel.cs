using BirdsCommon.Repository;
using BirdsCommon.ViewModelBase;

namespace BirdsViewModels
{
    public class EditSpecieViewModel : ViewModelBase
    {
        public Specie? Specie{ get => Get<Specie>(); set => Set(value); }
        /// <summary>Имя.</summary>
        public string? Name { get => Get<string?>(); set => Set(value); }

        protected override void OnPropertyChanged(string propertyName, object? oldValue, object? newValue)
        {
            base.OnPropertyChanged(propertyName, oldValue, newValue);
            if (propertyName == nameof(Specie))
            {
                Name = Specie?.Name;
            }
        }
    }
}
