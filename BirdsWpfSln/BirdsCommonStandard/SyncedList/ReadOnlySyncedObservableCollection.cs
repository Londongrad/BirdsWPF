using System.Collections.ObjectModel;
using System.Threading;

namespace BirdsCommonStandard
{
    public class ReadOnlySyncedObservableCollection<T> : ReadOnlyObservableCollection<T>, ISyncedList
    {
        public ReaderWriterLock ReaderWriterLocker { get; }
        public ReadOnlySyncedObservableCollection(SyncedObservableCollection<T> list)
             : base(list)
        {
            ReaderWriterLocker = list.ReaderWriterLocker;
        }
    }
}
