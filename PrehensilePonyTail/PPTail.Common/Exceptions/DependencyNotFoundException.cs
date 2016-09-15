using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Exceptions
{
    public class DependencyNotFoundException : System.Exception
    {
        public Type InterfaceType { get; set; }

        public DependencyNotFoundException(Type interfaceType) : base($"Unable to locate dependency for {interfaceType.FullName}")
        {
            this.InterfaceType = interfaceType;
        }

    }
}
