using BirdsCommonStandard;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace BirdsCommon
{
    public static partial class CollectionBindingHelper
    {
        /// <summary>
        /// Нужно чтобы ObservableCollection можно было обновлять из любого потока. <br/>
        /// Без этого методы обновления БД в Репозитории будут выкидывать исключения. <br/>
        /// Это специфика платформы WPF. В UWP, например, в таком нет необходимости.
        /// </summary>
        /// <param name="collection"></param>
        public static void EnableCollectionSynchronization(this ICollection collection)
            => EnableCollectionSynchronization(collection, Timeout.InfiniteTimeSpan);

        public static void EnableCollectionSynchronization(this ICollection collection, TimeSpan timeout)
        {
            if (collection is null)
                return;
            if (SynchronizationContext.Current is DispatcherSynchronizationContext context)
            {
                if (collection is ISyncedList syncCollection)
                {
                    syncCollection.EnableCollectionSynchronization(timeout);
                }
                else
                {
                    BindingOperations.EnableCollectionSynchronization(collection, collection.SyncRoot);
                }
            }
            else
            {
                Application.Current.Dispatcher.Invoke(collection.EnableCollectionSynchronization);
            }
        }

        public static void DisableCollectionSynchronization(this ICollection collection)
        {
            if (collection is null)
                return;
            if (SynchronizationContext.Current is DispatcherSynchronizationContext context)
            {
                BindingOperations.DisableCollectionSynchronization(collection);
            }
            else
            {
                Application.Current.Dispatcher.Invoke(collection.DisableCollectionSynchronization);
            }
        }
        public static void EnableCollectionSynchronization<T>(this ISyncedList collection)
            => EnableCollectionSynchronization(collection, TimeSpan.FromSeconds(1));
        public static void EnableCollectionSynchronization(this ISyncedList syncCollection, TimeSpan timeout)
        {
            if (syncCollection is null)
                return;
            if (SynchronizationContext.Current is DispatcherSynchronizationContext context)
            {
                TimeSpanClass spanClass = new TimeSpanClass() { Time = timeout };
                times.AddOrUpdate(syncCollection, spanClass);
                BindingOperations.EnableCollectionSynchronization(syncCollection, spanClass, OnCollectionSynchronization);
            }
            else
            {
                Application.Current.Dispatcher.Invoke(syncCollection.EnableCollectionSynchronization);
            }
            static void OnCollectionSynchronization(IEnumerable collection, object context, Action accessMethod, bool writeAccess)
            {
                ISyncedList syncCollection = (ISyncedList)collection;

                TimeSpan timeout = ((TimeSpanClass)context).Time;

                CollectionSynchronization(syncCollection, timeout, accessMethod, writeAccess);
            }
        }

        private static readonly ConditionalWeakTable<ISyncedList, TimeSpanClass> times = new ConditionalWeakTable<ISyncedList, TimeSpanClass>();

        private class TimeSpanClass
        {
            public TimeSpan Time { get; set; }
        }

        public static void CollectionSynchronization(ISyncedList syncCollection, TimeSpan timeout, Action accessMethod, bool writeAccess)
        {
            if (writeAccess)
            {
                syncCollection.ReaderWriterLocker.AcquireWriterLock(timeout);
                accessMethod();
                syncCollection.ReaderWriterLocker.ReleaseWriterLock();
            }
            else
            {
                syncCollection.ReaderWriterLocker.AcquireReaderLock(timeout);
                accessMethod();
                syncCollection.ReaderWriterLocker.ReleaseReaderLock();
            }
        }
    }

}
