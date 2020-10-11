using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Threading;
using Newtonsoft.Json;
using RestSharp;
using SimpleLed;
using Image = System.Drawing.Image;
using Timer = System.Timers.Timer;

namespace Driver.Razer
{
    public class RazerDriver : ISimpleLedWithConfig
    {
        public event EventHandler DeviceRescanRequired;

        [JsonIgnore]
        public RazerConfigModel configModel = new RazerConfigModel();

        public bool GetIsDirty()
        {
            return configModel.DataIsDirty;
        }

        public void SetIsDirty(bool val)
        {
            configModel.DataIsDirty = val;
        }

        public const string initJson = "{\"title\":\"RGB Sync Studio\",\"description\":\"The next generation of RGB control software.\",\"author\":{\"name\":\"Fanman03\",\"contact\":\"www.rgbsync.com\"},\"device_supported\":[\"keyboard\",\"mouse\",\"headset\",\"mousepad\",\"keypad\",\"chromalink\"],\"category\":\"application\"}";

        public static string uri;

        public static int sessionId;


        public async void Configure(DriverDetails driverDetails)
        {
            InitResponse response = PostAsync<InitResponse>("http://localhost:54235/razer/chromasdk", initJson).Result;
            uri = response.uri;

            await Task.Delay(TimeSpan.FromMilliseconds(100));

            Timer heartbeatTimer = new Timer();
            heartbeatTimer.Elapsed += new ElapsedEventHandler(heartbeat_Tick);
            heartbeatTimer.Interval = 1000;
            heartbeatTimer.Enabled = true;
            GC.KeepAlive(heartbeatTimer);
        }

        private void heartbeat_Tick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(uri)) return;
            
            string heartbeatUri = uri + "/heartbeat";
            var client = new RestClient(heartbeatUri);
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            IRestResponse tickResponse = client.Execute(request);
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
            RazerConfigModel data = this.configModel;
            SLSConfigData proxy = data;
            return (T)proxy;
        }

        public UserControl GetCustomConfig(ControlDevice controlDevice)
        {
            var config = new RazerConfig()
            {
                DataContext = configModel
            };

            configModel.CurrentControlDevice = controlDevice;

            return config;
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
                SupportsCustomConfig = true,
                Id = Guid.Parse("9594242f-ac1b-4cae-b6b6-24d1482d3a09"),
                Author = "Fanman03",
                Blurb = "Driver for all devices compatible with the Razer Chroma SDK.",
                CurrentVersion = new ReleaseNumber(1, 0, 0, 3),
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
            RazerConfigModel proxy = config as RazerConfigModel;
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

        private async Task<T> PostAsync<T>(string url, string model)
        {
            using (var client = new HttpClient())
            {
                var data = new StringContent(model, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, data);
                string result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(result);
            }
        }

        public static void Put(string url, Model.LedDataObject model)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(model);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PutAsync(url, data).Result;
            }
        }


    }
}
