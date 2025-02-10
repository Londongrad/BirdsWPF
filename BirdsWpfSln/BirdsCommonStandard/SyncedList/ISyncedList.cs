using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace BirdsCommonStandard
{
    public interface ISyncedList<T> : IList<T>
    {
        ReaderWriterLock ReaderWriterLocker { get; }
    }
    public interface ISyncedList : IList
    {
        ReaderWriterLock ReaderWriterLocker { get; }
    }
}
