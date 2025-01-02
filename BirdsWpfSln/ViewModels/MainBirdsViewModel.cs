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
        private readonly ObservableCollection<Bird> privateBirds;
        private static readonly ReadOnlyCollection<string> privateBirdNameGroups
            = Array.AsReadOnly(["Nuthatch", "Great tit", "Black-capped chickadee", "Sparrow", "Amadina", "All of them", "Only inactive", "Only active"]);
        #endregion

        public MainBirdsViewModel(IRepository<Bird> birdsRepository)
        {
            this.birdsRepository = birdsRepository;
            birdsVM = new(birdsRepository);
            Current = addBirdVM = new(birdsRepository);
            addBirdVM = new(birdsRepository);
            //privateBirds = birdsRepository.GetObservableCollection();
            //Birds = new(privateBirds);
            Birds = birdsRepository.GetObservableCollection();
        }

        public async Task LoadAsync() => await birdsRepository.LoadAsync();


        //private readonly ObservableCollection<Bird> privateBirds;
        public ReadOnlyObservableCollection<Bird> Birds { get; }

        private static readonly ReadOnlyCollection<string> privateBirdNameGroups
            = Array.AsReadOnly(["Nuthatch", "Great tit", "Black-capped chickadee", "Sparrow", "Amadina", "All of them", "Only inactive", "Only active"]);

        /// <summary>Ïðåäîñòàâëÿåò ñòàòè÷åñêóþ êîëëåêöèþ <see cref="privateBirdNameGroups"/>. 
        /// Ìîæíî áûëî îáîéòèñü ñòàòè÷åñêèì ïîëåì, íî äëÿ îáëåã÷åíèÿ ïðèâÿçîê ñîçäàíî ýòî ïðîêñè ñâîéñòâî.</summary>
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

        public RelayCommand AddBirdCommand => GetCommand<Bird>
        (
            async bird =>
            {
                await birdsRepository.AddAsync(bird);
            },
            bird => !string.IsNullOrWhiteSpace(bird.Name)
        );
        public RelayCommand DeleteBirdCommand => GetCommand<Bird>
        (
            async bird =>
            {
                bird.Departure = Departure;
                bird.IsActive = false;
                await birdsRepository.UpdateAsync(bird);
            },
            bird => bird.IsActive
        );
        #endregion
    }
}
