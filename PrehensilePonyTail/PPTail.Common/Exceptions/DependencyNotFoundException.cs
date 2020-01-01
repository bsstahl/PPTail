using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Exceptions
{
    public class DependencyNotFoundException : System.Exception
    {
        public String InterfaceTypeName { get; set; }

        public DependencyNotFoundException(String interfaceTypeName) : base($"Unable to locate dependency for type '{interfaceTypeName}'")
        {
            this.InterfaceTypeName = interfaceTypeName;
        }

        public DependencyNotFoundException(String interfaceTypeName, String instanceTypeName) 
            : base($"Unable to locate an instance of '{instanceTypeName}' that implements '{interfaceTypeName}'. Please verify that the Provider specified in the ConnectionString is available.")
        {
            this.InterfaceTypeName = interfaceTypeName;
        }

    }
}
