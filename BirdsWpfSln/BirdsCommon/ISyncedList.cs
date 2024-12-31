using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace BirdsCommon
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
