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
            var client = new RestClient(uri+"/mouse");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\r\n    \"effect\":\"CHROMA_CUSTOM2\",\r\n    \"param\":[\r\n        [ 255,255,255,255,255,255,255 ],\r\n        [ 65280,65280,65280,65280,65280,65280,65280 ],\r\n        [ 16711680,16711680,16711680,16711680,16711680,16711680,16711680 ],\r\n        [ 65535,65535,65535,65535,65535,65535,65535 ],\r\n        [ 16711935,16711935,16711935,16711935,16711935,16711935,16711935 ],\r\n        [ 255,255,255,255,255,255,255 ],\r\n        [ 65280,65280,65280,65280,65280,65280,65280 ],\r\n        [ 16711680,16711680,16711680,16711680,16711680,16711680,16711680 ],\r\n        [ 255,255,255,255,255,255,255 ]\r\n    ]\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }
        public static ControlDevice Device()
        {
            ControlDevice mouseControlDevice = new ControlDevice();
            mouseControlDevice.Driver = new RazerDriver();
            mouseControlDevice.DeviceType = DeviceTypes.Mouse;
            mouseControlDevice.Name = "Mouse";
            mouseControlDevice.Has2DSupport = true;
            mouseControlDevice.GridHeight = 9;
            mouseControlDevice.GridWidth = 7;
            mouseControlDevice.LEDs = new ControlDevice.LedUnit[63];

            for (int i = 0; i < 63; i++)
            {
                mouseControlDevice.LEDs[i] = new ControlDevice.LedUnit
                {
                    LEDName = "LED " + i.ToString(),
                    Data = new ControlDevice.LEDData
                    {
                        LEDNumber = i
                    }
                };
            }

            return mouseControlDevice;
        }
    }
}
