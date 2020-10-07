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
            int[] colors = new int[20];
            for (int i = 0; i < 20; i++)
            {
                colors[i] = RazerDriver.ToBgr(controlDevice.LEDs[i].Color);
            }

            var client = new RestClient(uri + "/keypad");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\r\n    \"effect\":\"CHROMA_CUSTOM\",\r\n    \"param\":[\r\n        [ " + colors[0] + "," + colors[1] + "," + colors[2] + "," + colors[3] + "," + colors[4] + " ],\r\n        [ " + colors[5] + "," + colors[6] + "," + colors[7] + "," + colors[8] + "," + colors[9] + " ],\r\n        [ " + colors[10] + "," + colors[11] + "," + colors[12] + "," + colors[13] + "," + colors[14] + " ],\r\n        [ " + colors[15] + "," + colors[16] + "," + colors[17] + "," + colors[18] + "," + colors[19] + " ]\r\n    ]\r\n}", ParameterType.RequestBody);
            client.Execute(request);
        }
        public static ControlDevice Device()
        {
            ControlDevice keypadControlDevice = new ControlDevice();
            keypadControlDevice.Driver = new RazerDriver();
            keypadControlDevice.DeviceType = DeviceTypes.Keypad;
            keypadControlDevice.Name = "Keypad";
            keypadControlDevice.ProductImage = RazerDriver.GetImage("Keypad");
            keypadControlDevice.Has2DSupport = true;
            keypadControlDevice.GridHeight = 4;
            keypadControlDevice.GridWidth = 5;
            keypadControlDevice.LEDs = new ControlDevice.LedUnit[20];

            for (int i = 0; i < 20; i++)
            {
                int xPos = 0;
                int yPos = 0;

                if (xPos > 4)
                {
                    xPos = 0;
                    yPos++;
                }

                keypadControlDevice.LEDs[i] = new ControlDevice.LedUnit
                {
                    LEDName = "LED " + i.ToString(),
                    Data = new ControlDevice.PositionalLEDData()
                    {
                        X = xPos,
                        Y = yPos,
                        LEDNumber = i
                    }
                };
                xPos++;
            }

            return keypadControlDevice;
        }
    }
}