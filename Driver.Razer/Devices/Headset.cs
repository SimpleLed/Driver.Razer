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
   

    public class HeadSetDevice : RazerControlDevice
    {
        private readonly string baseUrl;

        public override string UpdateUrl => baseUrl + "/headset";

        public override Model.LedDataObject GetUpdateModel()
        {
            return Model.LedData("CHROMA_CUSTOM", this.LEDs);
        }

        public HeadSetDevice(string url, ISimpleLed driver)
        {
            baseUrl = url;
            Driver = driver;
            DeviceType = DeviceTypes.Headset;
            Name = "Headset";
            ProductImage = RazerDriver.GetImage("Headset");
            Has2DSupport = false;
            LEDs = new LedUnit[5];

            for (int i = 0; i < 5; i++)
            {
                LEDs[i] = new LedUnit
                {
                    LEDName = "LED " + i.ToString(),
                    Data = new LEDData
                    {
                        LEDNumber = i
                    }
                };
            }
        }
    }
}
