using System;
using System.Collections.Generic;
using System.Text;

namespace Prescription.Domain.Snapshot
{
    public interface ISnapshotProvider<T>
    {
        T GetSnapshot();
    }

    public interface ISnapshotReciever<T>
    {
        void LoadFromSnapshot(T snapshot);
    }

    public interface ISnapshotableEntity<T> : ISnapshotProvider<T>, ISnapshotReciever<T>
    { }
}
