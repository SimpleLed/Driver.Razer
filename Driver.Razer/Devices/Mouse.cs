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
    public static class Mouse
    {
        public static void UpdateLighting(ControlDevice controlDevice, string uri)
        {
            int[] colors = new int[63];
            for (int i = 0; i < 63; i++)
            {
                colors[i] = RazerDriver.ToBgr(controlDevice.LEDs[i].Color);
            }

            var client = new RestClient(uri+"/mouse");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\r\n    \"effect\":\"CHROMA_CUSTOM2\",\r\n    \"param\":[\r\n        [ " + colors[0] + "," + colors[1] + "," + colors[2] + "," + colors[3] + "," + colors[4] + "," + colors[5] + "," + colors[6] + " ],\r\n        [ " + colors[7] + "," + colors[8] + "," + colors[9] + "," + colors[10] + "," + colors[11] + "," + colors[12] + "," + colors[13] + " ],\r\n        [ " + colors[14] + "," + colors[15] + "," + colors[16] + "," + colors[17] + "," + colors[18] + "," + colors[19] + "," + colors[20] + " ],\r\n        [ " + colors[21] + "," + colors[22] + "," + colors[23] + "," + colors[24] + "," + colors[25] + "," + colors[26] + "," + colors[27] + " ],\r\n        [ " + colors[28] + "," + colors[29] + "," + colors[30] + "," + colors[31] + "," + colors[32] + "," + colors[33] + "," + colors[34] + " ],\r\n        [ " + colors[35] + "," + colors[36] + "," + colors[37] + "," + colors[38] + "," + colors[39] + "," + colors[40] + "," + colors[41] + " ],\r\n        [ " + colors[42] + "," + colors[43] + "," + colors[44] + "," + colors[45] + "," + colors[46] + "," + colors[47] + "," + colors[48] + " ],\r\n        [ " + colors[49] + "," + colors[50] + "," + colors[51] + "," + colors[52] + "," + colors[53] + "," + colors[54] + "," + colors[55] + " ],\r\n        [ " + colors[56] + "," + colors[57] + "," + colors[58] + "," + colors[59] + "," + colors[60] + "," + colors[61] + "," + colors[62] + " ]\r\n    ]\r\n}", ParameterType.RequestBody);
            client.Execute(request);
        }
        public static ControlDevice Device()
        {
            ControlDevice mouseControlDevice = new ControlDevice();
            mouseControlDevice.Driver = new RazerDriver();
            mouseControlDevice.DeviceType = DeviceTypes.Mouse;
            mouseControlDevice.Name = "Mouse";
            mouseControlDevice.ProductImage = RazerDriver.GetImage("Mouse");
            mouseControlDevice.Has2DSupport = true;
            mouseControlDevice.GridHeight = 9;
            mouseControlDevice.GridWidth = 7;
            mouseControlDevice.LEDs = new ControlDevice.LedUnit[63];

            for (int i = 0; i < 63; i++)
            {
                int xPos = 0;
                int yPos = 0;

                if (xPos > 6)
                {
                    xPos = 0;
                    yPos++;
                }
                mouseControlDevice.LEDs[i] = new ControlDevice.LedUnit
                {
                    LEDName = "LED " + i.ToString(),
                    Data = new ControlDevice.PositionalLEDData()
                    {
                        X =  xPos,
                        Y = yPos,
                        LEDNumber = i
                    }
                };
                xPos++;
            }

            return mouseControlDevice;
        }
    }
}
