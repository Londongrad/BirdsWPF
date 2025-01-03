using BirdsCommon;
using BirdsCommon.Repository;
using System.Collections.ObjectModel;

namespace BirdsViewModels
{
    public class MainBirdsViewModel : ViewModelBase, INavigationService
    {
        #region Fields
        private readonly IRepository<Bird> birdsRepository;
        private readonly BirdsViewModel birdsVM;
        private readonly AddBirdViewModel addBirdVM;
        private static readonly ReadOnlyCollection<string> privateBirdNameGroups
            = Array.AsReadOnly(["Nuthatch", "Great tit", "Black-capped chickadee", "Sparrow", "Amadina", "All of them", "Only inactive", "Only active"]);
        #endregion

        public MainBirdsViewModel(IRepository<Bird> birdsRepository)
        {
            this.birdsRepository = birdsRepository;
            birdsVM = new(birdsRepository);
            Current = addBirdVM = new(birdsRepository);
            addBirdVM = new(birdsRepository);

            Birds = birdsRepository.GetObservableCollection();
        }
        /// <summary>Предоставляет статическую коллекцию <see cref="privateBirdNameGroups"/>. 
        /// Можно было обойтись статическим полем, но для облегчения привязок создано это прокси свойство.</summary>
        public ReadOnlyCollection<string> BirdNameGroups => privateBirdNameGroups;

        #region Properties
        public ReadOnlyObservableCollection<Bird> Birds { get; }
        public object? Current { get => Get<object>(); private set => Set(value); }
        public DateOnly Departure { get; } = DateOnly.FromDateTime(DateTime.Now);
        #endregion

        #region Methods
        public void NavigateTo(object? viewModel)
        {
            Current = viewModel;
        }
        public async Task LoadAsync() => await birdsRepository.LoadAsync();
        #endregion

        #region Commands
        public RelayCommand NavigateToType => GetCommand<Type>(t =>
        {
            object? curr;
            if (t == typeof(BirdsViewModel))
            {
                curr = birdsVM;
            }
            else if (t == typeof(AddBirdViewModel))
            {
                curr = addBirdVM;
            }
            else
            {
                curr = null;
            }
            NavigateTo(curr);
        });

        public RelayCommand AddBirdCommand => GetCommand<AddBirdViewModel>
        (
            async birdVM =>
            {
                await birdsRepository.AddAsync(new Bird(birdVM.Id, birdVM.Name, birdVM.Description, birdVM.Arrival, birdVM.Departure, birdVM.IsActive));
                birdVM.Clear();
            },
            birdVM => !string.IsNullOrWhiteSpace(birdVM.Name)
        );
        public RelayCommand DeleteBirdCommand => GetCommand<Bird>
        (
            async bird =>
            {

                // Создание клона с внесёнными изменениями, которые отобразятся только после сохранения в Репозитории.
                Bird bird1 = new (bird.Id, bird.Name, bird.Description, bird.Arrival, bird.Departure, false);

                await birdsRepository.UpdateAsync(bird1);
            },
            bird => bird.IsActive
        );
        public RelayCommand RemoveBirdCommand => GetCommand<Bird>
        (
            async bird =>
            {
                await birdsRepository.DeleteAsync(bird.Id);
            }
        );
        #endregion
    }
}
