using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using SimpleLed;

namespace Driver.Razer.Devices
{
   

    public class MousepadDevice : RazerControlDevice
    {
        private readonly string baseUrl;

        public override string UpdateUrl => baseUrl + "/mousepad";

        public override Model.LedDataObject GetUpdateModel()
        {
            return Model.LedData("CHROMA_CUSTOM", this.LEDs);
        }

        public MousepadDevice(string url, ISimpleLed driver)
        {
            baseUrl = url;
            Driver = driver;
            DeviceType = DeviceTypes.MousePad;
            Name = "Mousepad";
            ProductImage = RazerDriver.GetImage("Mousepad");
            Has2DSupport = false;
            LEDs = new ControlDevice.LedUnit[15];

            for (int i = 0; i < 15; i++)
            {
                LEDs[i] = new ControlDevice.LedUnit
                {
                    LEDName = "LED " + i.ToString(),
                    Data = new ControlDevice.LEDData
                    {
                        LEDNumber = i
                    }
                };
            }

        }
    }
}