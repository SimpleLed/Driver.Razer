using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimpleLed;

namespace Driver.Razer.Devices
{
   

    public class ChromaLinkDevice : RazerControlDevice
    {
        private readonly string baseUrl;

        public override string UpdateUrl => baseUrl + "/chromalink";

        public ChromaLinkDevice(string url, ISimpleLed driver)
        {
            baseUrl = url;

            Driver = driver;
            DeviceType = DeviceTypes.Other;
            Name = "Chroma Link";
            ProductImage = RazerDriver.GetImage("ChromaLink");
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

        public override Model.LedDataObject GetUpdateModel()
        {
            return Model.LedData("CHROMA_CUSTOM", this.LEDs);
        }
    }
}