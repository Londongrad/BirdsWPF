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

        private readonly IBirdsModel model;

        private readonly BirdViewModel birdVM;
        private readonly SpeciesViewModel speciesVM;

        private readonly AddBirdViewModel addBirdVM;
        private readonly AddSpecieViewModel addSpecieVM;

        private readonly EditSpecieViewModel editSpecieVM;

        private static readonly ReadOnlyCollection<string> birdNamesToAdd
             = Array.AsReadOnly(["Поползень", "Большак", "Гайка", "Воробей", "Амадин", "Дубонос", "Щегол"]);
        private static readonly ReadOnlyCollection<string> birdNamesToShow
            = Array.AsReadOnly(["Поползень", "Большак", "Гайка", "Воробей", "Амадин", "Дубонос", "Щегол", "Показать всех", "Только активные", "Только неактивные"]);
        private readonly INavigationService navigator;
        #endregion

        public MainBirdsViewModel(IBirdsModel model)
        {
            this.model = model;

            birdVM = new(model.Birds);
            speciesVM = new(model.Species);

            addBirdVM = new();
            addSpecieVM = new();

            editSpecieVM = new();

            Birds = model.Birds.GetObservableCollection();
            Species = model.Species.GetObservableCollection();

            navigator = this;
            navigator.AddCreator(typeof(BirdViewModel), () => this.birdVM);
            navigator.AddCreator(typeof(AddBirdViewModel), () => this.addBirdVM);
            navigator.AddCreator(typeof(SpeciesViewModel), () => this.speciesVM);
            navigator.AddCreator(typeof(AddSpecieViewModel), () => this.addSpecieVM);
            navigator.AddCreator(typeof(EditSpecieViewModel), () => this.editSpecieVM);
            navigator.NavigateTo(addBirdVM);
        }

        /// <summary>Предоставляет статическую коллекцию <see cref="privateBirdNameGroups"/>. 
        /// Можно было обойтись статическим полем, но для облегчения привязок создано это прокси свойство.</summary>
        public ReadOnlyCollection<string> BirdNamesToAdd => birdNamesToAdd;
        public ReadOnlyCollection<string> BirdNamesToShow => birdNamesToShow;

        #region Properties
        public ReadOnlyObservableCollection<Bird> Birds { get; }
        public ReadOnlyObservableCollection<Specie> Species { get; }

        //Для чего это свойство?
        //public string? Name { get => Get<string>(); set => Set(value); }

        public DateOnly Departure { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        #endregion

        #region Methods
        public async Task LoadAsync()
        {
            await model.LoadAsync();
            //NumberOfBirds = Birds.Count;
        }

        public void RaiseCurrentChanged() => RaisePropertyChanged(nameof(INavigationService.Current));

        #endregion

        #region Commands
        public RelayCommand AddBirdCommand => GetCommand<AddBirdViewModel>
        (
            async birdVM =>
            {
                await model.Birds.AddAsync(new Bird(birdVM.Id, birdVM.Name!, birdVM.Description, birdVM.Arrival, birdVM.Departure, birdVM.IsActive, birdVM.SpecieId));
                //NumberOfBirds++;
            },
            birdVM => !string.IsNullOrWhiteSpace(birdVM.Name)
        );

        public RelayCommand DeleteBirdCommand => GetCommand<Bird>
        (
            async bird =>
            {
                // Создание клона с внесёнными изменениями, которые отобразятся только после сохранения в Репозитории.
                Bird bird1 = new(bird.Id, bird.Name, bird.Description, bird.Arrival, Departure, false, bird.SpecieId);

                await model.Birds.UpdateAsync(bird1);
            },
            bird => bird.IsActive
        );
        public RelayCommand RemoveBirdCommand => GetCommand<Bird>
        (
            async bird =>
            {
                await model.Birds.DeleteAsync(bird.Id);
                //NumberOfBirds--;
            }
        );

        public RelayCommand AddSpecieCommand => GetCommand<string>
        (
           async name => { addSpecieVM.Name = string.Empty; await model.Species.AddAsync(new Specie(0, name)); },
           name => ! Species.Any(x => x.Name == name)
        );

        public RelayCommand EditSpecieCommand => GetCommand<Specie>
        (
           sp => { editSpecieVM.Specie = sp; navigator.NavigateTo(editSpecieVM); }
        );

        public RelayCommand UpdateSpecieCommand => GetCommand<EditSpecieViewModel>
        (
           async eVM => await model.Species.UpdateAsync(new Specie(eVM.Specie!.Id, eVM.Name!)),
           eVM => ! (string.IsNullOrWhiteSpace(eVM.Name) || Species.Any(x => x.Name == eVM.Name))
        );

        #endregion

        public Specie? GetSpecie(int id) => Species.FirstOrDefault(sp => sp.Id == id);
    }
}
