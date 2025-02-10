using System;

namespace BirdsCommonStandard
{
    public interface ICloneable<T> : ICloneable
    {
        new T Clone();
    }
}
