using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Newtonsoft.Json;
using RestSharp;
using SimpleLed;

namespace Driver.Razer
{
    public class RazerDriver : ISimpleLed
    {
        public event EventHandler DeviceRescanRequired;

        private ControlDevice.LedUnit[] leds = new ControlDevice.LedUnit[5];

        public string uri;

        public int sessionId;

        public void Configure(DriverDetails driverDetails)
        {
            var client = new RestClient("http://localhost:54235/razer/chromasdk");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\r\n    \"title\": \"RGB Sync Studio\",\r\n    \"description\": \"The next generation of RGB control software.\",\r\n    \"author\": {\r\n        \"name\": \"Fanman03\",\r\n        \"contact\": \"www.rgbsync.com\"\r\n    },\r\n    \"device_supported\": [\r\n        \"keyboard\",\r\n        \"mouse\",\r\n        \"headset\",\r\n        \"mousepad\",\r\n        \"keypad\",\r\n        \"chromalink\"\r\n    ],\r\n    \"category\": \"application\"\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            InitResponse responseObj = JsonConvert.DeserializeObject<InitResponse>(response.Content);
            sessionId = responseObj.sessionid;
            uri = responseObj.uri;
            Console.WriteLine(uri);

            DispatcherTimer heartbeatTimer = new DispatcherTimer();
            heartbeatTimer.Tick += new EventHandler(heartbeat_Tick);
            heartbeatTimer.Interval = new TimeSpan(0, 0, 1);
            heartbeatTimer.Start();
        }

        private void heartbeat_Tick(object sender, EventArgs e)
        {
            var client = new RestClient(uri+"/heartbeat");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            client.Execute(request);
        }

        public void Dispose()
        {
            var client = new RestClient(uri);
            client.Timeout = -1;
            var request = new RestRequest(Method.DELETE);
            client.Execute(request);
        }

        public T GetConfig<T>() where T : SLSConfigData
        {
            throw new NotImplementedException();
        }

        public List<ControlDevice> GetDevices()
        {

            List<ControlDevice> devices = new List<ControlDevice>();

            devices.Add(Devices.Keyboard.Device());
            devices.Add(Devices.Mouse.Device());
            devices.Add(Devices.Mouse.Device());
            devices.Add(Devices.Headset.Device());
            devices.Add(Devices.ChromaLink.Device());

            return devices;
        }

        public DriverProperties GetProperties()
        {
            return new DriverProperties
            {
                SupportsPull = false,
                SupportsPush = true,
                IsSource = false,
                SupportsCustomConfig = false,
                Id = Guid.Parse("9594242f-ac1b-4cae-b6b6-24d1482d3a09"),
                Author = "Fanman03",
                Blurb = "Driver for all devices compatible with the Razer Chroma SDK.",
                CurrentVersion = new ReleaseNumber(1, 0, 0, 0),
                GitHubLink = "https://github.com/SimpleLed/Driver.Razer",
                IsPublicRelease = true
            };
        }

        public string Name()
        {
            return "Razer";
        }

        public void Pull(ControlDevice controlDevice)
        {
            throw new NotImplementedException();
        }

        public void Push(ControlDevice controlDevice)
        {
            switch (controlDevice.DeviceType)
            {
                case DeviceTypes.Keyboard:
                    Devices.Keyboard.UpdateLighting(controlDevice, uri);
                    break;
                case DeviceTypes.Mouse:
                    Devices.Mouse.UpdateLighting(controlDevice, uri);
                    break;
                case DeviceTypes.MousePad:
                    Devices.Mousepad.UpdateLighting(controlDevice, uri);
                    break;
                case DeviceTypes.Headset:
                    Devices.Headset.UpdateLighting(controlDevice, uri);
                    break;
                default:
                    Devices.ChromaLink.UpdateLighting(controlDevice, uri);
                    break;
            }
        }

        public void PutConfig<T>(T config) where T : SLSConfigData
        {
            throw new NotImplementedException();
        }

        public class InitResponse
        {
            public int sessionid { get; set; }

            public string uri { get; set; }
        }
    }
}
