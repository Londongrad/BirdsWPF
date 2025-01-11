using BirdsCommon;
using BirdsCommon.Command;
using BirdsCommon.Repository;
using BirdsCommon.ViewModelBase;
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
            = Array.AsReadOnly(["Поползень", "Большак", "Гайка", "Воробей", "Амадин", "Дубонос", "Щегол"]);
        private readonly INavigationService navigator;
        #endregion

        public MainBirdsViewModel(IRepository<Bird> birdsRepository)
        {
            this.birdsRepository = birdsRepository;
            birdsVM = new(birdsRepository);
            addBirdVM = new();
            Birds = birdsRepository.GetObservableCollection();
            navigator = this;
            navigator.NavigateTo(addBirdVM = new());
            navigator.AddCreator(typeof(BirdsViewModel), () => this.birdsVM);
            navigator.AddCreator(typeof(AddBirdViewModel), () => this.addBirdVM);
            
        }

        /// <summary>Предоставляет статическую коллекцию <see cref="privateBirdNameGroups"/>. 
        /// Можно было обойтись статическим полем, но для облегчения привязок создано это прокси свойство.</summary>
        public ReadOnlyCollection<string> BirdNameGroups => privateBirdNameGroups;

        #region Properties
        public ReadOnlyObservableCollection<Bird> Birds { get; }

        public int NumberOfBirds { get => Get<int>(); set => Set(value); }
        public DateOnly Departure { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        #endregion

        #region Methods
        public async Task LoadAsync()
        {
            await birdsRepository.LoadAsync();
            NumberOfBirds = Birds.Count;
        } 

        public void RaiseCurrentChanged() => RaisePropertyChanged(nameof(INavigationService.Current));

        #endregion

        #region Commands
        public RelayCommand AddBirdCommand => GetCommand<AddBirdViewModel>
        (
            async birdVM =>
            {
                await birdsRepository.AddAsync(new Bird(birdVM.Id, birdVM.Name, birdVM.Description, birdVM.Arrival, birdVM.Departure, birdVM.IsActive));
                NumberOfBirds++;
            },
            birdVM => !string.IsNullOrWhiteSpace(birdVM.Name)
        );

        public RelayCommand DeleteBirdCommand => GetCommand<Bird>
        (
            async bird =>
            {
                // Создание клона с внесёнными изменениями, которые отобразятся только после сохранения в Репозитории.
                Bird bird1 = new(bird.Id, bird.Name, bird.Description, bird.Arrival, Departure, false);

                await birdsRepository.UpdateAsync(bird1);
            },
            bird => bird.IsActive
        );
        public RelayCommand RemoveBirdCommand => GetCommand<Bird>
        (
            async bird =>
            {
                await birdsRepository.DeleteAsync(bird.Id);
                NumberOfBirds--;
            }
        );
        #endregion
    }
}
