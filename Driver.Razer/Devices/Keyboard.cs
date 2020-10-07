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
    public static class Keyboard
    {
        public static void UpdateLighting(ControlDevice controlDevice, string uri)
        {
                var client = new RestClient(uri + "/keyboard");
                client.Timeout = -1;
                var request = new RestRequest(Method.PUT);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", "{\r\n    \"effect\": \"CHROMA_STATIC\",\r\n    \"param\": {\r\n        \"color\": 1677215\r\n    }\r\n}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
        }
        public static ControlDevice Device()
        {
            ControlDevice keyboardControlDevice = new ControlDevice();
            keyboardControlDevice.Driver = new RazerDriver();
            keyboardControlDevice.DeviceType = DeviceTypes.Keyboard;
            keyboardControlDevice.Name = "Keyboard";
            keyboardControlDevice.ProductImage = RazerDriver.GetImage("Keyboard");
            keyboardControlDevice.Has2DSupport = true;
            keyboardControlDevice.GridHeight = 6;
            keyboardControlDevice.GridWidth = 22;
            keyboardControlDevice.LEDs = new ControlDevice.LedUnit[132];

            for (int i = 0; i < 132; i++)
            {
                keyboardControlDevice.LEDs[i] = new ControlDevice.LedUnit
                {
                    LEDName = "LED " + i.ToString(),
                    Data = new ControlDevice.LEDData
                    {
                        LEDNumber = i
                    }
                };
            }

            return keyboardControlDevice;
        }
    }
}
