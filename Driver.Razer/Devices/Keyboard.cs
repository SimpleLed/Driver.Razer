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
            int[] colors = new int[132];
            for (int i = 0; i < 132; i++)
            {
                colors[i] = RazerDriver.ToBgr(controlDevice.LEDs[i].Color);
            }

            var client = new RestClient(uri + "/keyboard");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\r\n    \"effect\":\"CHROMA_CUSTOM\",\r\n    \"param\":[\r\n        [ " + colors[0] + "," + colors[1] + "," + colors[2] + "," + colors[3] + "," + colors[4] + "," + colors[5] + "," + colors[6] + "," + colors[7] + ", " + colors[8] + ", " + colors[9] + ", " + colors[10] + ", " + colors[11] + ", " + colors[12] + ", " + colors[13] + "," + colors[14] + ", " + colors[15] + ", " + colors[16] + ", " + colors[17] + ", " + colors[18] + ", " + colors[19] + ", " + colors[20] + "," + colors[21] + " ],\r\n        [ " + colors[22] + "," + colors[23] + "," + colors[24] + "," + colors[25] + "," + colors[26] + "," + colors[27] + "," + colors[28] + "," + colors[29] + ", " + colors[30] + ", " + colors[31] + ", " + colors[32] + ", " + colors[33] + ", " + colors[34] + ", " + colors[35] + "," + colors[36] + ", " + colors[37] + ", " + colors[38] + ", " + colors[39] + ", " + colors[40] + ", " + colors[41] + ", " + colors[42] + "," + colors[43] + " ],\r\n        [ " + colors[44] + "," + colors[45] + "," + colors[46] + "," + colors[47] + "," + colors[48] + "," + colors[49] + "," + colors[50] + "," + colors[51] + ", " + colors[52] + ", " + colors[53] + ", " + colors[54] + ", " + colors[55] + ", " + colors[56] + ", " + colors[57] + "," + colors[58] + ", " + colors[59] + ", " + colors[60] + ", " + colors[61] + ", " + colors[62] + ", " + colors[63] + ", " + colors[64] + "," + colors[65] + " ],\r\n        [ " + colors[66] + "," + colors[67] + "," + colors[68] + "," + colors[69] + "," + colors[70] + "," + colors[71] + "," + colors[72] + "," + colors[73] + ", " + colors[74] + ", " + colors[75] + ", " + colors[76] + ", " + colors[77] + ", " + colors[78] + ", " + colors[79] + "," + colors[80] + ", " + colors[81] + ", " + colors[82] + ", " + colors[83] + ", " + colors[84] + ", " + colors[85] + ", " + colors[86] + "," + colors[87] + " ],\r\n        [ " + colors[88] + "," + colors[89] + "," + colors[90] + "," + colors[91] + "," + colors[92] + "," + colors[93] + "," + colors[94] + "," + colors[95] + ", " + colors[96] + ", " + colors[97] + ", " + colors[98] + ", " + colors[99] + ", " + colors[100] + ", " + colors[101] + "," + colors[102] + ", " + colors[103] + ", " + colors[104] + ", " + colors[105] + ", " + colors[106] + ", " + colors[107] + ", " + colors[108] + "," + colors[109] + " ],\r\n        [ " + colors[110] + "," + colors[111] + "," + colors[112] + "," + colors[113] + "," + colors[114] + "," + colors[115] + "," + colors[116] + "," + colors[117] + ", " + colors[118] + ", " + colors[119] + ", " + colors[120] + ", " + colors[121] + ", " + colors[122] + ", " + colors[123] + "," + colors[124] + ", " + colors[125] + ", " + colors[126] + ", " + colors[127] + ", " + colors[128] + ", " + colors[129] + ", " + colors[130] + "," + colors[131] + " ]\r\n    ]\r\n}", ParameterType.RequestBody);
            client.Execute(request);
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
                int xPos = 0;
                int yPos = 0;

                if (xPos > 21)
                {
                    xPos = 0;
                    yPos++;
                }
                keyboardControlDevice.LEDs[i] = new ControlDevice.LedUnit
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

            return keyboardControlDevice;
        }
    }
}
