using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Exceptions
{
    public class DependencyNotFoundException : System.Exception
    {
        public string InterfaceTypeName { get; set; }

        public DependencyNotFoundException(string interfaceTypeName) : base($"Unable to locate dependency for type '{interfaceTypeName}'")
        {
            this.InterfaceTypeName = interfaceTypeName;
        }

        public DependencyNotFoundException(string interfaceTypeName, string instanceTypeName) : base($"Unable to locate an instance of '{instanceTypeName}' that implements '{interfaceTypeName}'")
        {
            this.InterfaceTypeName = interfaceTypeName;
        }

    }
}
