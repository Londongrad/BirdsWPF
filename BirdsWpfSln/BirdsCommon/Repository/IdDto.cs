using System.Diagnostics.CodeAnalysis;

namespace BirdsCommon.Repository
{
    /// <summary>Базовый класс с идентификатором.</summary>
    public class IdDto(int id)
    {
        /// <summary>Целочисленный идентификатор.</summary>
        public int Id { get; } = id;

        public override int GetHashCode() => Id;

        public virtual bool ValueEquals(IdDto? other)
        {
            if (other is null)
                return false;
            return GetType() == other.GetType() && Id == other.Id;
        }

        public static bool ValueEquals(IdDto left, IdDto right)
        {
            if (left is null)
                return right is null;
            return left.ValueEquals(right);
        }
    }
}
