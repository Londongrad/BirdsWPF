using System.Collections;

namespace BirdsCommon.SyncedList
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
