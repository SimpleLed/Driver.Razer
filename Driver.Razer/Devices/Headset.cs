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
    public static class Headset
    {
        public static void UpdateLighting(ControlDevice controlDevice,string uri)
        {
            var client = new RestClient(uri+"/headset");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\r\n    \"effect\":\"CHROMA_CUSTOM\",\r\n    \"param\":[ 255, 255, 255, 255, 255 ]\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }
        public static ControlDevice Device()
        {
            ControlDevice headsetControlDevice = new ControlDevice();
            headsetControlDevice.Driver = new RazerDriver();
            headsetControlDevice.DeviceType = DeviceTypes.Headset;
            headsetControlDevice.Name = "Headset";
            headsetControlDevice.Has2DSupport = false;
            headsetControlDevice.LEDs = new ControlDevice.LedUnit[5];

            for (int i = 0; i < 5; i++)
            {
                headsetControlDevice.LEDs[i] = new ControlDevice.LedUnit
                {
                    LEDName = "LED " + i.ToString(),
                    Data = new ControlDevice.LEDData
                    {
                        LEDNumber = i
                    }
                };
            }

            return headsetControlDevice;
        }
    }
}
