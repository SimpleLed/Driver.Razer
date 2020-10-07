using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;
using Newtonsoft.Json;
using RestSharp;
using SimpleLed;
using Timer = System.Timers.Timer;

namespace Driver.Razer
{
    public class RazerDriver : ISimpleLed
    {
        public event EventHandler DeviceRescanRequired;

        private ControlDevice.LedUnit[] leds = new ControlDevice.LedUnit[5];

        public static string uri;

        public int sessionId;

        public void Configure(DriverDetails driverDetails)
        {
            Console.WriteLine("Connecting to Razer API...");
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

            Thread.Sleep(100);

            Timer heartbeatTimer = new Timer();
            heartbeatTimer.Elapsed += new ElapsedEventHandler(heartbeat_Tick);
            heartbeatTimer.Interval = 1000;
            heartbeatTimer.Enabled = true;
            GC.KeepAlive(heartbeatTimer);
        }

        private void heartbeat_Tick(object sender, EventArgs e)
        {
            if (uri == null)
            {
                Console.WriteLine("No URI");
            }
            else
            {
                string heartbeatUri = uri + "/heartbeat";
                var client = new RestClient(heartbeatUri);
                client.Timeout = -1;
                var request = new RestRequest(Method.PUT);
                IRestResponse tickResponse = client.Execute(request);
                //Console.WriteLine(tickResponse.Content);
            }
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
            devices.Add(Devices.Keypad.Device());
            devices.Add(Devices.Mouse.Device());
            devices.Add(Devices.Mousepad.Device());
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
                CurrentVersion = new ReleaseNumber(1, 0, 0, 2),
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
            if (uri == null)
            {
                Console.WriteLine("No URI");
            }
            else
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
                    case DeviceTypes.Keypad:
                        Devices.Keypad.UpdateLighting(controlDevice, uri);
                        break;
                    default:
                        Devices.ChromaLink.UpdateLighting(controlDevice, uri);
                        break;
                }
            }
        }

        public void PutConfig<T>(T config) where T : SLSConfigData
        {
            throw new NotImplementedException();
        }

        public static Bitmap GetImage(string image)
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();

            try
            {
                Stream imageStream = myAssembly.GetManifestResourceStream("Driver.Razer.ProductImages." + image + ".png");
                return (Bitmap)Image.FromStream(imageStream);
            }
            catch
            {
                Stream placeholder = myAssembly.GetManifestResourceStream("Driver.Razer.RazerPlaceholder.png");
                return (Bitmap)Image.FromStream(placeholder);
            }
        }

        public class InitResponse
        {
            public int sessionid { get; set; }

            public string uri { get; set; }
        }

        public static int ToBgr(LEDColor rgbColor)
        {
            // Return a zero-alpha 24-bit BGR color integer
            return (0 << 24) + (rgbColor.Blue << 16) + (rgbColor.Green << 8) + rgbColor.Red;
        }

    }
}
