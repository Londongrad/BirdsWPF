using BirdsCommon;
using BirdsCommon.Repository;
using System.Collections.ObjectModel;

namespace BirdsWPF.ViewModels
{
    public class BirdsViewModel : ViewModelBase
    {
        #region [ Fields ]
        private readonly IRepository<Bird> birdRepository;
        #endregion

        public BirdsViewModel(IRepository<Bird> birdRepository)
        {
            this.birdRepository = birdRepository;
            Birds = birdRepository.GetObservableCollection();
        }

        public async Task LoadAsync() => await birdRepository.LoadAsync();


        #region [ Properties ]
        public ObservableCollection<Bird> Birds { get; }
        public string? Name { get; set; }
        public List<string> BirdsNames => ["Nuthatch", "Great tit", "Black-capped chickadee", "Sparrow", "Amadina", "All of them", "Only inactive", "Only active"];
        public DateOnly Departure { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public int NumberOfBirds { get => Get<int>(); set => Set(value); }
        #endregion

        #region [ Commands ]
        public RelayCommand DeleteBirdCommand => GetCommand<Bird>
        (
            async bird =>
            {
                // Здесь нужна индикация выполнения метода?

                Bird bird1 = bird.Clone();
                bird1.Departure = Departure;
                bird1.IsActive = false;

                Birds.Replace(b => b == bird, bird1);

                Bird bird2 = await birdRepository.UpdateAsync(bird);
                Birds.Replace(b => b == bird1, bird2);
            },
            bird => bird.IsActive
        );
        #endregion

        #region [ Methods ]
        //private async void LoadBirds()
        //{
        //    var birds = await birdRepository.GetAllAsync();
        //    Birds.Clear();
        //    foreach (var bird in birds)
        //        Birds.Add(bird);

        //    #region [ Здесь будет выборка по свойству Name, которое будет браться из ComboBox ]
        //    //switch (Name)
        //    //{
        //    //    case "Только неактивные":
        //    //        foreach (var bird in birds.Where(b => b.IsAlive == false))
        //    //            Birds.Add(bird);
        //    //        break;
        //    //    case "Только активные":
        //    //        foreach (var bird in birds.Where(b => b.IsAlive == true))
        //    //            Birds.Add(bird);
        //    //        break;
        //    //    case "Поползень":
        //    //    case "Большак":
        //    //    case "Гайка":
        //    //    case "Воробей":
        //    //    case "Амадин":
        //    //        foreach (var bird in birds.Where(b => b.Name == Name))
        //    //            Birds.Add(bird);
        //    //        break;
        //    //    default:
        //    //        foreach (var bird in birds)
        //    //            Birds.Add(bird);
        //    //        break;
        //    //}
        //    #endregion

        //    NumberOfBirds = Birds.Count;
        //}

        #region [ Тут будет команда по показу сущностей в зависимости от выбранного из ComboBox ]
        //private void ShowOnly()
        //{
        //    LoadBirds();
        //}
        #endregion

        #endregion
    }
}