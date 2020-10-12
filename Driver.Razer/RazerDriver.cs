using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Driver.Razer.Devices;
using Newtonsoft.Json;
using SimpleLed;
using Image = System.Drawing.Image;
using Timer = System.Timers.Timer;

namespace Driver.Razer
{
    public class RazerDriver : ISimpleLedWithConfig
    {

        private readonly List<USBDevice> supportedKeyboards = RazorHIDS.RazorUsbDevices.Where(x => x.DeviceType == DeviceTypes.Keyboard).ToList();
        private readonly List<USBDevice> supportedMice = RazorHIDS.RazorUsbDevices.Where(x => x.DeviceType == DeviceTypes.Mouse).ToList();
        private readonly List<USBDevice> supportedMouseMats = RazorHIDS.RazorUsbDevices.Where(x => x.DeviceType == DeviceTypes.MousePad).ToList();
        private readonly List<USBDevice> supportedHeadsets = RazorHIDS.RazorUsbDevices.Where(x => x.DeviceType == DeviceTypes.Headset).ToList();
        private readonly List<USBDevice> supportedKeypads = new List<USBDevice>
        {
            //todo - update the device types of keyboards that are actually keypads
        };

        private List<USBDevice> supportedChromalinks = new List<USBDevice>
        {
            //todo - wtf are these?
        };

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
            Startup();
        }

        public async Task Startup()
        {
            InitResponse response = await RESTHelpers.PostAsync<InitResponse>("http://localhost:54235/razer/chromasdk", initJson).ConfigureAwait(false);
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

            try
            {
                RESTHelpers.Put(heartbeatUri);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("heartbeat failed: " + ex.Message);
            }

        }

        public void Dispose()
        {
            if (string.IsNullOrWhiteSpace(uri)) return;

            RESTHelpers.Delete(uri);
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

            var connectedKeyboards = SLSManager.GetSupportedDevices(supportedKeyboards);
            var connectedMice = SLSManager.GetSupportedDevices(supportedMice);
            var connectedMouseMats = SLSManager.GetSupportedDevices(supportedMouseMats);
            var connectedKeypads = SLSManager.GetSupportedDevices(supportedKeypads);
            var connectedHeadsets = SLSManager.GetSupportedDevices(supportedHeadsets);
            var connectedChromaLinks = SLSManager.GetSupportedDevices(supportedChromalinks);


            if (!configModel.ShowOnlyConnected || connectedKeyboards.Any())
            {
                KeyboardDevice keyboard = new KeyboardDevice(uri, this);

                if (connectedKeyboards.Any())
                {
                    USBDevice first = connectedKeyboards.First();

                    keyboard.Name = first.DevicePrettyName;

                    if (first.HID.HasValue)
                    {
                        KeyboardHelper.AddKeyboardWatcher(first.VID, first.HID.Value, keyboard.HandleInput);
                    }
                }

                devices.Add(keyboard);
            }

            if (!configModel.ShowOnlyConnected || connectedMice.Any())
            {
                MouseDevice mouse = new MouseDevice(uri, this);

                if (connectedMice.Any())
                {
                    USBDevice first = connectedMice.First();

                    mouse.Name = first.DevicePrettyName;
                }

                devices.Add(mouse);
            }

            if (!configModel.ShowOnlyConnected || connectedMouseMats.Any())
            {
                MousepadDevice mousePad = new MousepadDevice(uri, this);

                if (connectedMouseMats.Any())
                {
                    USBDevice first = connectedMouseMats.First();

                    mousePad.Name = first.DevicePrettyName;
                }

                devices.Add(mousePad);
            }


            if (!configModel.ShowOnlyConnected || connectedKeypads.Any())
            {
                KeypadDevice keypad = new KeypadDevice(uri, this);

                if (connectedKeypads.Any())
                {
                    USBDevice first = connectedKeypads.First();

                    keypad.Name = first.DevicePrettyName;
                }

                devices.Add(keypad);
            }



            if (!configModel.ShowOnlyConnected || connectedHeadsets.Any())
            {
                HeadSetDevice headset = new HeadSetDevice(uri, this);

                if (connectedHeadsets.Any())
                {
                    USBDevice first = connectedHeadsets.First();

                    headset.Name = first.DevicePrettyName;
                }

                devices.Add(headset);
            }

            if (!configModel.ShowOnlyConnected || connectedChromaLinks.Any())
            {
                ChromaLinkDevice chromaLink = new ChromaLinkDevice(uri, this);

                if (connectedChromaLinks.Any())
                {
                    USBDevice first = connectedChromaLinks.First();

                    chromaLink.Name = first.DevicePrettyName;
                }

                devices.Add(chromaLink);
            }

            return devices;
        }

        public DriverProperties GetProperties()
        {

            var allSupported = supportedChromalinks.ToList();
            allSupported.AddRange(supportedHeadsets);
            allSupported.AddRange(supportedKeyboards);
            allSupported.AddRange(supportedKeypads);
            allSupported.AddRange(supportedMice);
            allSupported.AddRange(supportedMouseMats);
            return new DriverProperties
            {
                SupportsPull = false,
                SupportsPush = true,
                IsSource = false,
                SupportsCustomConfig = true,
                Id = Guid.Parse("9594242f-ac1b-4cae-b6b6-24d1482d3a09"),
                Author = "Fanman03",
                Blurb = "Driver for all devices compatible with the Razer Chroma SDK.",
                CurrentVersion = new ReleaseNumber(1, 0, 0, 5),
                GitHubLink = "https://github.com/SimpleLed/Driver.Razer",
                IsPublicRelease = true,
                SupportedDevices = allSupported
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
                RazerControlDevice razerControlDevice = controlDevice as RazerControlDevice;

                RESTHelpers.Put(razerControlDevice.UpdateUrl, razerControlDevice.GetUpdateModel());
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
                if (imageStream != null)
                {
                    return (Bitmap)Image.FromStream(imageStream);
                }
            }
            catch
            {
            }

            Stream placeholder = myAssembly.GetManifestResourceStream("Driver.Razer.RazerPlaceholder.png");
            if (placeholder != null)
            {
                return (Bitmap)Image.FromStream(placeholder);
            }

            return null;
        }

        public class InitResponse
        {
            public int sessionid { get; set; }

            public string uri { get; set; }
        }

        public static int ToBgr(LEDColor rgbColor)
        {
            // Return a zero-alpha 24-bit BGR color integer
            return (rgbColor.Blue << 16) + (rgbColor.Green << 8) + rgbColor.Red;
        }

        public static int[][] ToJaggedArray(ControlDevice.LedUnit[] leds, int width, int height)
        {
            int[] colors = leds.Select(x => RazerDriver.ToBgr(x.Color)).ToArray();
            int[][] twodColors = new int[height][];
            int ptr = 0;
            for (int i = 0; i < height; i++)
            {
                twodColors[i] = new int[width];
                for (int p = 0; p < width; p++)
                {
                    twodColors[i][p] = colors[ptr];
                    ptr++;
                }
            }

            return twodColors;
        }
    }
}
