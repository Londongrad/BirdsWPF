using BirdsCommon;

namespace BirdsWPF.Models
{
    public class BirdEntity : ViewModelBase
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateOnly Arrival { get => Get<DateOnly>(); set => Set(value); }
        public DateOnly Departure { get => Get<DateOnly>(); set => Set(value); }
        public bool IsActive { get => Get<bool>(); set => Set(value); }
    }
}
