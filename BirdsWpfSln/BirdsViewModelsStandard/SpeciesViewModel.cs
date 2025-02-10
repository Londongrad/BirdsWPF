﻿using BirdsCommonStandard;
using System;

namespace BirdsViewModels
{
    public class SpeciesViewModel : ViewModelBase
    {
        public SpeciesViewModel(ViewModelSettings settings)
            : base(settings)
        { }

        //#region [ Fields ]
        //private readonly IRepository<Bird> birdRepository;
        //#endregion

        //public BirdsViewModel(IRepository<Bird> birdRepository)
        //{
        //    this.birdRepository = birdRepository;
        //    //Birds = birdRepository.GetObservableCollection();
        //}


        #region [ Properties ]
        public DateTime Departure { get; } = DateTime.Now;

        ///// <summary>Какое-то непонятное свойство. Для чего оно? <br/>
        ///// Это свойство определяло общее количесво сущностей в коллекции с последующим выводом на экран. <br/>
        ///// Я конкатенировал это свойство со строкой Number of Birds: <br/>
        ///// В результате выходило это $"Number of Birds: {NumberOfBirds}" <br/>
        ///// Выводилось все это добро в BirdsView.
        ///// </summary>
        //public int NumberOfBirds { get => Get<int>(); set => Set(value); }
        #endregion
    }
}