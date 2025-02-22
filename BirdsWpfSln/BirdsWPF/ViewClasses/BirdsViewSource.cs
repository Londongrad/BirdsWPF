using BirdsCommon;
using BirdsCommon.Repository;
using System.Collections;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace BirdsWPF.ViewClasses
{
    /// <summary>Кастомизация <see cref="CollectionViewSource"/> для работы с коллекциями <see cref="Bird"/>.
    /// Добавлены свойства для фильтрации по <see cref="Bird.SpecieId"/> и наличию фрагментов
    /// текста во всех свойствах <see cref="Bird"/>.</summary>
    public class BirdsViewSource : CollectionViewSource
    {
        protected override void OnSourceChanged(object oldSource, object newSource)
        {
            if (newSource is not null && newSource is not IEnumerable<Bird>)
                throw new NotImplementedException("Источником может быть только коллекция реализующая IEnumerable<Bird>.");

            base.OnSourceChanged(oldSource, newSource);

        }

        public BirdsViewSource()
        {
            // Задание параметров фильтрации CollectionViewSource:
            // динамическая фильтрация при изменении любого свойства Bird. 
            IsLiveFilteringRequested = true;
            LiveFilteringProperties.AddRange(PropertyNames);
        }

        private static readonly ReadOnlyCollection<PropertyInfo> Properties = Array.AsReadOnly(typeof(Bird).GetProperties().ToArray());
        private static readonly ReadOnlyCollection<string> PropertyNames = Array.AsReadOnly(Properties.Select(prop => prop.Name).ToArray());

        public IEnumerable<Specie> SelectedSpecies
        {
            get { return (IEnumerable<Specie>)GetValue(SelectedSpeciesProperty); }
            set { SetValue(SelectedSpeciesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedSpecies.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedSpeciesProperty =
            DependencyProperty.Register(nameof(SelectedSpecies),
                                        typeof(IEnumerable<Specie>),
                                        typeof(BirdsViewSource),
                                        new PropertyMetadata(null)
                                        {
                                            PropertyChangedCallback = OnSpeciesChanged,
                                            CoerceValueCallback = static (d, e) => e ?? Array.Empty<Specie>()
                                        });

        private static void OnSpeciesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            BirdsViewSource birdsView = (BirdsViewSource)d;
            if (e.NewValue is not IEnumerable<Specie> species || !species.Any())
                birdsView.ids = null;
            else
                birdsView.ids = species.Select(sp => sp.Id).ToImmutableHashSet();
            Refilter(birdsView, birdsView.ids, birdsView.words);
        }

        private FilterEventHandler? filterHandler;
        private ImmutableHashSet<int>? ids;
        private ImmutableArray<string>? words;

        private static void Refilter(BirdsViewSource birdsView, ImmutableHashSet<int>? ids, ImmutableArray<string>? words)
        {
            if (birdsView.filterHandler is not null)
            {
                birdsView.Filter -= birdsView.filterHandler;
            }


            switch (ids is null, words is null)
            {
                case (true, true):
                    birdsView.filterHandler = null;
                    break;
                case (false, true):
                    birdsView.filterHandler = (_, e) =>
                    {
                        Bird bird = (Bird)e.Item;
                        e.Accepted &= ids!.Contains(bird.SpecieId);
                    };
                    break;
                case (true, false):
                    birdsView.filterHandler = (_, e) =>
                    {
                        Bird bird = (Bird)e.Item;
                        foreach (string word in words!)
                        {
                            if (!Properties.Any(prop => prop.GetValue(bird) is object obj && obj.ToString() is string str && str.Contains(word, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                e.Accepted = false;
                                break;
                            }
                        }
                    };
                    break;
                default:
                    birdsView.filterHandler = (_, e) =>
                    {
                        Bird bird = (Bird)e.Item;

                        if (e.Accepted)
                        {
                            if (!ids!.Contains(bird.SpecieId))
                            {
                                e.Accepted = false;
                            }
                        }

                        if (e.Accepted)
                        {
                            foreach (string word in words!)
                            {
                                if (!Properties.Any(prop => prop.GetValue(bird) is object obj && obj.ToString() is string str && str.Contains(word, StringComparison.InvariantCultureIgnoreCase)))
                                {
                                    e.Accepted = false;
                                    break;
                                }
                            }
                        }
                    };
                    break;
            }

            if (birdsView.filterHandler is not null)
            {
                birdsView.Filter += birdsView.filterHandler;
            }
        }


        /// <summary>Возвращает привязанный к <see cref="Selector"/> экземпляр <see cref="BirdsViewSource"/>.</summary>
        /// <param name="selector"><see cref="Selector"/> к которуму привязан экземпляр <see cref="BirdsViewSource"/>.</param>
        /// <returns><see cref="BirdsViewSource"/> который привязан к <paramref name="selector"/> или <see langword="null"/>.</returns>
        public static BirdsViewSource GetBirdsSource(Selector selector)
        {
            return (BirdsViewSource)selector.GetValue(BirdsSourceProperty);
        }

        /// <summary>Задаёт привязку экземпляра <see cref="BirdsViewSource"/> к <see cref="Selector"/>.</summary>
        /// <param name="selector"><see cref="Selector"/> к которому надо привязать экземпляр <see cref="BirdsViewSource"/>.</param>
        /// <param name="birdsSource">Экземпляр <see cref="BirdsViewSource"/>, который привязывается к <paramref name="selector"/> или <see langword="null"/>.</param>
        /// <remarks>Если <paramref name="birdsSource"/> <see langword="null"/>, то происходит отвязка ранее заданного экземпляра <see cref="BirdsViewSource"/>.</remarks>
        public static void SetBirdsSource(Selector selector, BirdsViewSource? birdsSource)
        {
            selector.SetValue(BirdsSourceProperty, birdsSource);
        }

        // Using a DependencyProperty as the backing store for SelectedSpeciesSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BirdsSourceProperty =
            DependencyProperty.RegisterAttached(nameof(BirdsSourceProperty)[0..^8],
                                                typeof(BirdsViewSource),
                                                typeof(BirdsViewSource),
                                                new PropertyMetadata(null)
                                                {
                                                    PropertyChangedCallback = OnBirdsSourceChanged
                                                });

        private static void OnBirdsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Selector selector)
                throw new NotImplementedException("Реализовано только для Selector");

            // Прослушка SelectionChanged
            selector.SelectionChanged -= OnSelectionChanged;
            if (e.NewValue is BirdsViewSource)
            {
                selector.SelectionChanged += OnSelectionChanged;
                OnSelectionChanged(selector, null!);
            }
        }

        private static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selector selector = (Selector)sender;
            BirdsViewSource birdsViewSource = GetBirdsSource(selector);

            if (selector.ReadLocalValue(ListBox.SelectedItemsProperty) != DependencyProperty.UnsetValue)
            {
                // Получение из SelectedItems
                birdsViewSource.SelectedSpecies = ((IList)selector.GetValue(ListBox.SelectedItemsProperty)).OfType<Specie>().ToArray();
            }
            else
            {
                // Получение из SelectedItem
                birdsViewSource.SelectedSpecies = selector.SelectedItem is Specie specie
                    ? [specie]
                    : [];
            }
        }



        public string TextFilter
        {
            get { return (string)GetValue(TextFilterProperty); }
            set { SetValue(TextFilterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextFilter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextFilterProperty =
            DependencyProperty.Register(nameof(TextFilter),
                                        typeof(string),
                                        typeof(BirdsViewSource),
                                        new PropertyMetadata(string.Empty)
                                        {
                                            CoerceValueCallback = (_, value) => value ?? string.Empty,
                                            PropertyChangedCallback = OnTextFilterChanged
                                        });

        private static void OnTextFilterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BirdsViewSource birdsView = (BirdsViewSource)d;
            string[] words = ((string)e.NewValue).Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (words.Length == 0)
                birdsView.words = null;
            else
                birdsView.words = words.ToImmutableArray();
            Refilter(birdsView, birdsView.ids, birdsView.words);
        }
    }

    /// <summary>Вспомогательный класс для обхода автопреобразования в привязках
    /// класса CollectionViewSource в класс CollectionView.</summary>
    public class TextFilterHelper
    {
        public string? Text { get; set; }
    }
}
