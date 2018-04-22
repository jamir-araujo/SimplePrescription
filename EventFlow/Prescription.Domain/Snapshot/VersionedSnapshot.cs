using System;
using System.Collections.Generic;
using System.Text;
using EventFlow.Snapshots;

namespace Prescription.Domain.Snapshot
{
    public abstract class VersionedSnapshot : ISnapshot
    {
        public int Version { get; }

        protected VersionedSnapshot(int version)
        {
            Version = version;
        }
    }
}
