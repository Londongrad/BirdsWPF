using BirdsWPF.Core;
using BirdsWPF.Models;
using BirdsWPF.Repositories.Abstract;

namespace BirdsWPF.ViewModels
{
    public class AddBirdViewModel(IBirdRepository birdRepository) : ObservableObject
    {
        #region [ Fields ]
        private RelayCommand? _addBirdCommand;
        #endregion

        #region [ Properties ]
        public string? Name { get; set; }
        public DateOnly Arrival { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string? Description { get; set; }
        public List<string> Birds => ["Nuthatch", "Great tit", "Black-capped chickadee", "Sparrow", "Amadina", "Gold finch"];
        #endregion

        #region [ Commands ]
        public RelayCommand AddBirdCommand => _addBirdCommand ??= new RelayCommand
        (
            async _ =>
            {
                var bird = new BirdEntity()
                {
                    Name = Name,
                    Description = Description,
                    Arrival = Arrival
                };
                await birdRepository.AddAsync(bird);
            },
            bird => Name != ""
        );
        #endregion
    }
}
