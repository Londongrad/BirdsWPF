namespace BirdsCommon.Cloneable
{
    public interface ICloneable<T> : ICloneable
    {
        new T Clone();
    }
}
