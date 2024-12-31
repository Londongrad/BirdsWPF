
using BirdsCommon;
using BirdsCommon.Repository;
using System.Collections.ObjectModel;

namespace BirdsViewModels
{
    public class MainBirdsViewModel : ViewModelBase, INavigationService
    {
        private readonly IRepository<Bird> birdsRepository;
        private readonly BirdsViewModel birdsVM;
        private readonly AddBirdViewModel addBirdVM;

        public MainBirdsViewModel(IRepository<Bird> birdsRepository)
        {
            this.birdsRepository = birdsRepository;
            birdsVM = new(birdsRepository);
            addBirdVM = new(birdsRepository);
            privateBirds = birdsRepository.GetObservableCollection();
            Birds = new(privateBirds);
        }

        public async Task LoadAsync() => await birdsRepository.LoadAsync();


        private readonly ObservableCollection<Bird> privateBirds;
        public ReadOnlyObservableCollection<Bird> Birds { get; }

        private static readonly ReadOnlyCollection<string> privateBirdNameGroups
            = Array.AsReadOnly(["Nuthatch", "Great tit", "Black-capped chickadee", "Sparrow", "Amadina", "All of them", "Only inactive", "Only active"]);

        /// <summary>Предоставляет статическую коллекцию <see cref="privateBirdNameGroups"/>. 
        /// Можно было обойтись статическим полем, но для облегчения привязок создано это прокси свойство.</summary>
        public ReadOnlyCollection<string> BirdNameGroups => privateBirdNameGroups;

        public DateOnly Departure { get; } = DateOnly.FromDateTime(DateTime.Now);
        public RelayCommand DeleteBirdCommand => GetCommand<Bird>
        (
            async bird =>
            {
                // Здесь нужна индикация выполнения метода?

                Bird bird1 = bird.Clone();
                bird1.Departure = Departure;
                bird1.IsActive = false;

                Bird bird2 = await birdsRepository.UpdateAsync(bird);
                //Birds.Replace(b => b == bird1, bird2);
            },
            bird => bird.IsActive
        );

        #region [ Properties ]
        public object? Current { get => Get<object>(); private set => Set(value); }
        #endregion

        #region [ Methods ]
        public void NavigateTo(object? viewModel)
        {
            Current = viewModel;
        }
        #endregion


        public RelayCommand NavigateToType => GetCommand<Type>(t =>
        {
            object? curr;
            if (t == typeof(BirdsViewModel))
                curr = birdsVM;
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

        public RelayCommand AddBirdCommand => GetCommand<Bird>
        (
            async bird =>
            {
                await birdsRepository.AddAsync(bird);
            },
            bird => !string.IsNullOrWhiteSpace(bird.Name)
        );

    }
}
