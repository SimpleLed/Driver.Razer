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
    public static class Keypad
    {
        public static void UpdateLighting(ControlDevice controlDevice, string uri)
        {
            var client = new RestClient(uri + "/keypad");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\r\n    \"effect\":\"CHROMA_CUSTOM\",\r\n    \"param\":[\r\n        [ 255,255,255,255,255 ],\r\n        [ 65280,65280,65280,65280,65280 ],\r\n        [ 16711680,16711680,16711680,16711680,16711680 ],\r\n        [ 65535,65535,65535,65535,65535 ]\r\n    ]\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }
        public static ControlDevice Device()
        {
            ControlDevice keypadControlDevice = new ControlDevice();
            keypadControlDevice.Driver = new RazerDriver();
            keypadControlDevice.DeviceType = DeviceTypes.Other;
            keypadControlDevice.Name = "Keypad";
            keypadControlDevice.ProductImage = RazerDriver.GetImage("Keypad");
            keypadControlDevice.Has2DSupport = true;
            keypadControlDevice.GridHeight = 4;
            keypadControlDevice.GridWidth = 5;
            keypadControlDevice.LEDs = new ControlDevice.LedUnit[20];

            for (int i = 0; i < 20; i++)
            {
                keypadControlDevice.LEDs[i] = new ControlDevice.LedUnit
                {
                    LEDName = "LED " + i.ToString(),
                    Data = new ControlDevice.LEDData
                    {
                        LEDNumber = i
                    }
                };
            }

            return keypadControlDevice;
        }
    }
}