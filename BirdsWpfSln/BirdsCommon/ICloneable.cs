namespace BirdsCommon
{
    public interface ICloneable<T> : ICloneable
    {
        new T Clone();
    }
}
