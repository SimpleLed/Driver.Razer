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
    public static class Mousepad
    {
        public static void UpdateLighting(ControlDevice controlDevice, string uri)
        {
            var client = new RestClient(uri + "/mousepad");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\r\n    \"effect\":\"CHROMA_CUSTOM\",\r\n    \"param\":[ 2255, 255, 255, 255, 255, 65280, 65280, 65280, 65280, 65280, 16711680, 16711680, 16711680, 16711680, 16711680, 16777215, 16777215, 16777215, 16777215, 16777215 ]\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }
        public static ControlDevice Device()
        {
            ControlDevice mousepadControlDevice = new ControlDevice();
            mousepadControlDevice.Driver = new RazerDriver();
            mousepadControlDevice.DeviceType = DeviceTypes.MousePad;
            mousepadControlDevice.Name = "Mousepad";
            mousepadControlDevice.ProductImage = RazerDriver.GetImage("Mousepad");
            mousepadControlDevice.Has2DSupport = false;
            mousepadControlDevice.LEDs = new ControlDevice.LedUnit[20];

            for (int i = 0; i < 20; i++)
            {
                mousepadControlDevice.LEDs[i] = new ControlDevice.LedUnit
                {
                    LEDName = "LED " + i.ToString(),
                    Data = new ControlDevice.LEDData
                    {
                        LEDNumber = i
                    }
                };
            }

            return mousepadControlDevice;
        }
    }
}