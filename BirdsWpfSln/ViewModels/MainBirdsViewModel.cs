using BirdsCommon;
using BirdsCommon.Command;
using BirdsCommon.Repository;
using BirdsCommon.ViewModelBase;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace BirdsViewModels
{
    public class MainBirdsViewModel : ViewModelBase, INavigationService
    {
        #region Fields
        private readonly IRepository<Bird> birdsRepository;
        private readonly BirdViewModel birdVM;
        private readonly AddBirdViewModel addBirdVM;
        private static readonly ReadOnlyCollection<string> birdNamesToAdd
            = Array.AsReadOnly(["Поползень", "Большак", "Гайка", "Воробей", "Амадин", "Дубонос", "Щегол"]);
        private static readonly ReadOnlyCollection<string> birdNamesToShow
            = Array.AsReadOnly(["Поползень", "Большак", "Гайка", "Воробей", "Амадин", "Дубонос", "Щегол", "Показать всех", "Только активные", "Только неактивные"]);
        private readonly INavigationService navigator;
        #endregion

        public MainBirdsViewModel(IRepository<Bird> birdsRepository)
        {
            this.birdsRepository = birdsRepository;
            birdVM = new(birdsRepository);
            addBirdVM = new();
            Birds = birdsRepository.GetObservableCollection();
            navigator = this;
            navigator.NavigateTo(addBirdVM = new());
            navigator.AddCreator(typeof(BirdViewModel), () => this.birdVM);
            navigator.AddCreator(typeof(AddBirdViewModel), () => this.addBirdVM);

            //Коллекция для сортировки.
            BirdsCollectionView = CollectionViewSource.GetDefaultView(Birds);
            BirdsCollectionView.SortDescriptions.Add(new SortDescription(nameof(Bird.IsActive), ListSortDirection.Descending));
            BirdsCollectionView.SortDescriptions.Add(new SortDescription(nameof(Bird.Arrival), ListSortDirection.Descending));
        }

        /// <summary>Предоставляет статическую коллекцию <see cref="privateBirdNameGroups"/>. 
        /// Можно было обойтись статическим полем, но для облегчения привязок создано это прокси свойство.</summary>
        public ReadOnlyCollection<string> BirdNamesToAdd => birdNamesToAdd;
        public ReadOnlyCollection<string> BirdNamesToShow => birdNamesToShow;

        #region Properties
        public ReadOnlyObservableCollection<Bird> Birds { get; set; }
        public string? Name { get => Get<string>(); set => Set(value); }
        public int NumberOfBirds { get => Get<int>(); set => Set(value); }
        public DateOnly Departure { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        /// <summary>
        /// Коллекция для сортировки
        /// </summary>
        public ICollectionView BirdsCollectionView { get; }
        #endregion

        #region Methods
        public async Task LoadAsync()
        {
            await birdsRepository.LoadAsync();
            NumberOfBirds = Birds.Count;
        }

        public void RaiseCurrentChanged() => RaisePropertyChanged(nameof(INavigationService.Current));
        private bool FilterByName(object obj)
        {
            if (obj is Bird bird)
            {
                switch (Name)
                {
                    case "Амадин":
                    case "Воробей":
                    case "Большак":
                    case "Гайка":
                    case "Поползень":
                    case "Дубонос":
                        return bird.Name!.Contains(Name);
                    case "Только активные":
                        return bird.IsActive == true;
                    case "Только неактивные":
                        return bird.IsActive == false;
                    default:
                        return true;
                }
            }
            return false;
        }
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
        /// <summary>
        /// Команда для сортировки коллекции в зависимости от свойства <see cref="Name"/> <br/>
        /// Значение свойства задается из ComboBox в окне BirdsView
        /// </summary>
        public RelayCommand ShowOnlyCommand => GetCommand
        (
            () =>
            {
                BirdsCollectionView.Filter = FilterByName;
                NumberOfBirds = BirdsCollectionView.Cast<Bird>().Count();
            },
            () => !string.IsNullOrEmpty(Name)
        );
        #endregion
    }
}
