using BirdsWPF.Core;

namespace BirdsWPF.Models
{
    public class BirdEntity : ObservableObject
    {
        private bool _isActive = true;
        private DateOnly _arrival = new();
        private DateOnly _departure = new();

        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateOnly Arrival { get => _arrival; set => Set(ref _arrival, value); }
        public DateOnly Departure { get => _departure; set => Set(ref _departure, value); }
        public bool IsActive { get => _isActive; set => Set(ref _isActive, value); }
    }
}
