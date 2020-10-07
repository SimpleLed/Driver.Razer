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
            int[] colors = new int[15];
            for (int i = 0; i < 15; i++)
            {
                colors[i] = RazerDriver.ToBgr(controlDevice.LEDs[i].Color);
            }
            var client = new RestClient(uri + "/mousepad");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\r\n    \"effect\":\"CHROMA_CUSTOM\",\r\n    \"param\":[ " + colors[0] + ", " + colors[1] + ", " + colors[2] + ", " + colors[3] + ", " + colors[4] + ", " + colors[5] + ", " + colors[6] + ", " + colors[7] + ", " + colors[8] + ", " + colors[9] + ", " + colors[10] + ", " + colors[11] + ", " + colors[12] + ", " + colors[13] + ", " + colors[14] + "]\r\n}", ParameterType.RequestBody);
            client.Execute(request);
        }
        public static ControlDevice Device()
        {
            ControlDevice mousepadControlDevice = new ControlDevice();
            mousepadControlDevice.Driver = new RazerDriver();
            mousepadControlDevice.DeviceType = DeviceTypes.MousePad;
            mousepadControlDevice.Name = "Mousepad";
            mousepadControlDevice.ProductImage = RazerDriver.GetImage("Mousepad");
            mousepadControlDevice.Has2DSupport = false;
            mousepadControlDevice.LEDs = new ControlDevice.LedUnit[15];

            for (int i = 0; i < 15; i++)
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