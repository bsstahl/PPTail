using PPTail.Entities;
using System;
using System.Collections.Generic;

namespace PPTail.Interfaces
{
    public interface IOutputRepository
    {
        string OutputLocation { get; }

        void Save(IEnumerable<SiteFile> files);
    }
}
