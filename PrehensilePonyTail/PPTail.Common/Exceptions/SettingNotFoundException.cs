using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Exceptions
{
    public class SettingNotFoundException:Exception
    {
        public String SettingName { get; set; }

        public SettingNotFoundException(String settingName)
            :base($"Required setting '{settingName}' was not provided.")
        {
            this.SettingName = settingName;
        }

        public SettingNotFoundException(String settingName, Exception innerException) 
            : base($"Required setting '{settingName}' was not loaded due to error '{innerException.Message}'.", innerException)
        {
            this.SettingName = settingName;
        }
    }
}
