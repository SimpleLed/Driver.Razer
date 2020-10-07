using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using SimpleLed;

namespace Driver.Razer.Devices
{
    public static class ChromaLink
    {
        public static void UpdateLighting(ControlDevice controlDevice, string uri)
        {
            int[] colors = new int[5];
            for (int i = 0; i < 5; i++)
            {
                colors[i] = RazerDriver.ToBgr(controlDevice.LEDs[i].Color);
            }
            var client = new RestClient(uri + "/chromalink");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\r\n    \"effect\":\"CHROMA_CUSTOM\",\r\n    \"param\":[ " + colors[0] + ", " + colors[1] + ", " + colors[2] + ", " + colors[3] + ", " + colors[4] + " ]\r\n}", ParameterType.RequestBody);
            client.Execute(request);
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