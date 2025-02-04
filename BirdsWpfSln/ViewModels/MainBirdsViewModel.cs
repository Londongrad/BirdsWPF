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
        private readonly ObservableCollection<BirdVM> privateBirds = new();
        private readonly Dictionary<int, BirdVM> dictBirds = new();
        #endregion

        public MainBirdsViewModel(IRepository<Bird> birdsRepository)
        {
            this.birdsRepository = birdsRepository;
            birdsVM = new(birdsRepository);
            Current = addBirdVM = new(birdsRepository);
            addBirdVM = new(birdsRepository);
            Birds = new ReadOnlyObservableCollection<BirdVM>(privateBirds);

            birdsRepository.RepChanged += OnbirdsRepositoryChanged;
        }

        private async void OnbirdsRepositoryChanged(object? sender, RepChangedArgs<Bird> e)
        {
            switch (e.Action)
            {
                case RepChangedAction.Reset:
                    await LoadAsync();
                    break;
                case RepChangedAction.Add:
                    {
                        BirdVM birdVM = new(e.NewItem!);
                        privateBirds.Add(birdVM);
                        dictBirds.Add(birdVM.Id, birdVM);
                    }
                    break;
                case RepChangedAction.Remove:
                    {
                        BirdVM birdVM = dictBirds[e.OldItem!.Id];
                        privateBirds.Remove(birdVM);
                        dictBirds.Remove(birdVM.Id);
                    }
                    break;
                case RepChangedAction.Update:
                    {
                        BirdVM birdVM = dictBirds[e.NewItem!.Id];
                        birdVM.SetBird(e.NewItem!);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>Предоставляет статическую коллекцию <see cref="privateBirdNameGroups"/>. 
        /// Можно было обойтись статическим полем, но для облегчения привязок создано это прокси свойство.</summary>
        public ReadOnlyCollection<string> BirdNameGroups => privateBirdNameGroups;

        #region Properties
        public ReadOnlyObservableCollection<BirdVM> Birds { get; }
        public object? Current { get => Get<object>(); private set => Set(value); }
        //public DateOnly Departure { get; } = DateOnly.FromDateTime(DateTime.Now);
        #endregion

        #region Methods
        public void NavigateTo(object? viewModel)
        {
            Current = viewModel;
        }

        public async Task LoadAsync()
        {
            IEnumerable<Bird> birds = await birdsRepository.GetAllAsync();
            privateBirds.Clear();
            dictBirds.Clear();
            foreach (Bird bird in birds)
            {
                BirdVM birdVM = new(bird);
                privateBirds.Add(birdVM);
                dictBirds.Add(birdVM.Id, birdVM);
            }
        }
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
        public RelayCommand DeleteBirdCommand => GetCommand<BirdVM>
        (
            async bird =>
            {

                // Создание клона с внесёнными изменениями, которые отобразятся только после сохранения в Репозитории.
                Bird bird1 = new(bird.Id, bird.Bird!.Name, bird.Bird.Description, bird.Bird.Arrival, bird.EditableDeparture, false);

                await birdsRepository.UpdateAsync(bird1);
            },
            bird => bird.Bird?.IsActive ?? false
        );
        public RelayCommand RemoveBirdCommand => GetCommand<BirdVM>
        (
            async bird =>
            {
                await birdsRepository.DeleteAsync(bird.Id);
            }
        );
        #endregion
    }
}
