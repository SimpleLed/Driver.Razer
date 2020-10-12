using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleLed;
using NotImplementedException = System.NotImplementedException;

namespace Driver.Razer.Devices
{
    public abstract class RazerControlDevice : InteractiveControlDevice
    {
        public abstract string UpdateUrl { get;}
        public abstract Model.LedDataObject GetUpdateModel();
    }
}
