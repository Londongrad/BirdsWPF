using BirdsCommonStandard;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BirdsViewModels
{
    public class MainBirdsViewModel : NavigationServiceBase
    {
        #region Fields
        private readonly IBirdsModel birdsModel;
        private readonly ViewModelSettings settings;
        private readonly IRepository<Bird> birdsRepository;
        private readonly IRepository<Specie> speciesRepository;
        private readonly BirdsViewModel birdsVM;
        private readonly AddBirdViewModel addBirdVM;
        private readonly SpeciesViewModel speciesVM;
        private readonly AddSpecieViewModel addSpecieVM;
        private readonly EditSpecieViewModel editSpecieVM;

        private static readonly ReadOnlyCollection<string> birdNamesToAdd
            = Array.AsReadOnly(new string[] { "Поползень", "Большак", "Гайка", "Воробей", "Амадин", "Дубонос", "Щегол" });

        private static readonly ReadOnlyCollection<string> birdNamesToShow
            = Array.AsReadOnly(new string[] { "Поползень", "Большак", "Гайка", "Воробей", "Амадин", "Дубонос", "Щегол", "Показать всех", "Только активные", "Только неактивные" });

        private readonly INavigationService navigator;
        #endregion

        public MainBirdsViewModel(IBirdsModel birdsModel, ViewModelSettings settings)
            : base(settings)
        {
            this.birdsModel = birdsModel ?? throw new ArgumentNullException(nameof(birdsModel));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

            birdsRepository = birdsModel.Birds;
            speciesRepository = birdsModel.Species;

            birdsVM = new BirdsViewModel(this.settings);
            addBirdVM = new AddBirdViewModel(this.settings);
            speciesVM = new SpeciesViewModel(this.settings);
            addSpecieVM = new AddSpecieViewModel(this.settings);
            editSpecieVM = new EditSpecieViewModel(this.settings);

            birdsRepository.RepChanged += OnBirdsChanged;
            speciesRepository.RepChanged += OnSpeciesChanged;

            navigator = this;
            navigator.AddCreator(typeof(BirdsViewModel), () => birdsVM);
            navigator.AddCreator(typeof(AddBirdViewModel), () => addBirdVM);
            navigator.AddCreator(typeof(SpeciesViewModel), () => speciesVM);
            navigator.AddCreator(typeof(AddSpecieViewModel), () => addSpecieVM);
            navigator.AddCreator(typeof(EditSpecieViewModel), () => editSpecieVM);
            navigator.NavigateToType(typeof(AddBirdViewModel));

            //// Коллекция для сортировки.
            // BirdsCollectionView = CollectionViewSource.GetDefaultView(Birds);
            // BirdsCollectionView.SortDescriptions.Add(new SortDescription(nameof(Bird.IsActive), ListSortDirection.Descending));
            // BirdsCollectionView.SortDescriptions.Add(new SortDescription(nameof(Bird.Arrival), ListSortDirection.Descending));
            // BirdsCollectionView.SortDescriptions.Add(new SortDescription(nameof(Bird.Name), ListSortDirection.Descending));
        }

        private async void OnBirdsChanged(object sender, RepChangedArgs<Bird> e)
        {
            switch (e.Action)
            {
                case RepChangedAction.Reset:
                    {
                        Birds.Clear();
                        IEnumerable<Bird> birds = await birdsRepository.GetAllAsync();
                        foreach (Bird bird in birds)
                        {
                            Birds.ReplaceOrAdd(bird);
                        }
                    }
                    break;
                case RepChangedAction.Add:
                case RepChangedAction.Update:
                    {
                        Birds.ReplaceOrAdd(e.NewItem);
                    }
                    break;
                case RepChangedAction.Remove:
                    {
                        Birds.Remove(e.OldItem);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private async void OnSpeciesChanged(object sender, RepChangedArgs<Specie> e)
        {
            switch (e.Action)
            {
                case RepChangedAction.Reset:
                    {
                        Species.Clear();
                        IEnumerable<Specie> species = await speciesRepository.GetAllAsync();
                        foreach (Specie specie in species)
                        {
                            Species.ReplaceOrAdd(specie);
                        }
                    }
                    break;
                case RepChangedAction.Add:
                case RepChangedAction.Update:
                    {
                        Species.ReplaceOrAdd(e.NewItem);
                    }
                    break;
                case RepChangedAction.Remove:
                    {
                        Species.Remove(e.OldItem);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }


        /// <summary>Предоставляет статическую коллекцию <see cref="privateBirdNameGroups"/>. 
        /// Можно было обойтись статическим полем, но для облегчения привязок создано это прокси свойство.</summary>
        public ReadOnlyCollection<string> BirdNamesToAdd => birdNamesToAdd;
        public ReadOnlyCollection<string> BirdNamesToShow => birdNamesToShow;

        #region Properties
        public IdDtoVMCollection<Bird, IdBirdVM> Birds { get; } = new IdDtoVMCollection<Bird, IdBirdVM>(bird => new IdBirdVM(bird));
        public IdDtoVMCollection<Specie, IdSpecieVM> Species { get; } = new IdDtoVMCollection<Specie, IdSpecieVM>(sp => new IdSpecieVM(sp));

        public string Name { get => Get<string>(); set => Set(value); }
        //public int NumberOfBirds { get => Get<int>(); set => Set(value); }
        public DateTime Departure { get; set; } = DateTime.Now;

        ///// <summary>
        ///// Коллекция для сортировки
        ///// </summary>
        //public ICollectionView BirdsCollectionView { get; }
        #endregion

        #region Methods
        public async Task LoadAsync() => await Task.Run(async () =>
        {
            Birds.Clear();
            Species.Clear();
            IEnumerable<Bird> birds = await birdsRepository.GetAllAsync();
            IEnumerable<Specie> species = await speciesRepository.GetAllAsync();

            foreach (Bird bird in birds)
            {
                Birds.ReplaceOrAdd(bird);
            }

            foreach (Specie specie in species)
            {
                Species.ReplaceOrAdd(specie);
            }
        });

        //private bool FilterByName(object obj)
        //{
        //    if (obj is Bird bird)
        //    {
        //        switch (Name)
        //        {
        //            case "Амадин":
        //            case "Воробей":
        //            case "Большак":
        //            case "Гайка":
        //            case "Поползень":
        //            case "Дубонос":
        //                return bird.Name!.Contains(Name);
        //            case "Только активные":
        //                return bird.IsActive == true;
        //            case "Только неактивные":
        //                return bird.IsActive == false;
        //            default:
        //                return true;
        //        }
        //    }
        //    return false;
        //}
        #endregion

        #region Commands
        public RelayCommand AddBirdCommand => GetCommand<AddBirdViewModel>
        (
            async birdVM =>
            {
                await birdsRepository.AddAsync(new Bird(birdVM.Id,
                                                        birdVM.Name,
                                                        birdVM.Description,
                                                        birdVM.Arrival,
                                                        birdVM.Departure,
                                                        birdVM.IsActive,
                                                        birdVM.SpecieId));
                //NumberOfBirds++;
            },
            birdVM => !string.IsNullOrWhiteSpace(birdVM.Name)
        );

        public RelayCommand DeleteBirdCommand => GetCommand<Bird>
        (
            async bird =>
            {
                // Создание клона с внесёнными изменениями, которые отобразятся только после сохранения в Репозитории.
                Bird bird1 = new Bird(bird.Id, bird.Name, bird.Description, bird.Arrival, Departure, false, bird.SpecieId);

                await birdsRepository.UpdateAsync(bird1);
            },
            bird => bird.IsActive
        );

        public RelayCommand RemoveBirdCommand => GetCommand<Bird>
        (
            async bird =>
            {
                await birdsRepository.DeleteAsync(bird.Id);
                //NumberOfBirds--;
            }
        );

        ///// <summary>
        ///// Команда для сортировки коллекции в зависимости от свойства <see cref="Name"/> <br/>
        ///// Значение свойства задается из ComboBox в окне BirdsView
        ///// </summary>
        //public RelayCommand ShowOnlyCommand => GetCommand
        //(
        //    () =>
        //    {
        //        BirdsCollectionView.Filter = FilterByName;
        //        NumberOfBirds = BirdsCollectionView.Cast<Bird>().Count();
        //    },
        //    () => !string.IsNullOrEmpty(Name)
        //);

        public RelayCommand AddSpecieCommand => GetCommand<string>
        (
            async name => await speciesRepository.AddAsync(new Specie(0, name)),
            string.IsNullOrWhiteSpace
        );

        public RelayCommand EditSpecieCommand => GetCommand<IdSpecieVM>
        (
            svm =>
            {
                editSpecieVM.SpecieId = svm.Id;
                editSpecieVM.Name = svm.ModelEntity.Name;
                navigator.NavigateTo(editSpecieVM);
            }
        );

        public RelayCommand UpdateSpecieCommand => GetCommand<EditSpecieViewModel>
        (
           async esvm =>
            {
                await speciesRepository.UpdateAsync(new Specie(esvm.SpecieId, esvm.Name));
                navigator.NavigateTo(speciesVM);
            },
            esvm => !string.IsNullOrEmpty(esvm.Name)
        );

        #endregion
    }
}
