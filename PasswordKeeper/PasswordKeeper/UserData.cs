using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordKeeper
{

    internal class UsbDriveInfo
    {
        public string ID_Device { get; private set; }
        public string ID_PNP { get; private set; }
        public string Volume { get; private set; }
        public string LogicalDisk { get; private set; }

        public UsbDriveInfo(string iD_Device, string iD_PNP, string volume, string logicalDisk)
        {
            ID_Device = iD_Device;
            ID_PNP = iD_PNP;
            Volume = volume;
            LogicalDisk = logicalDisk;
        }
    }

    internal class UserData
    {

    }
}
