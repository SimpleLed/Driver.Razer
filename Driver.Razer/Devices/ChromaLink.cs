using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using SimpleLed;

namespace Driver.Razer.Devices
{
    public static class ChromaLink
    {
        public static void UpdateLighting(ControlDevice controlDevice, string uri)
        {
            RazerDriver.Put(uri + "/chromalink",Model.LedData("CHROMA_CUSTOM", controlDevice.LEDs));
        }

        public static ControlDevice Device()
        {
            ControlDevice chromaLinkControlDevice = new ControlDevice();
            chromaLinkControlDevice.Driver = new RazerDriver();
            chromaLinkControlDevice.DeviceType = DeviceTypes.Other;
            chromaLinkControlDevice.Name = "Chroma Link";
            chromaLinkControlDevice.ProductImage = RazerDriver.GetImage("ChromaLink");
            chromaLinkControlDevice.Has2DSupport = false;
            chromaLinkControlDevice.LEDs = new ControlDevice.LedUnit[5];

            for (int i = 0; i < 5; i++)
            {
                chromaLinkControlDevice.LEDs[i] = new ControlDevice.LedUnit
                {
                    LEDName = "LED " + i.ToString(),
                    Data = new ControlDevice.LEDData
                    {
                        LEDNumber = i
                    }
                };
            }

            return chromaLinkControlDevice;
        }
    }
}