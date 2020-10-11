using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Newtonsoft.Json;
using SimpleLed;

namespace Driver.Razer.Devices
{
    

    public class KeyboardDevice : RazerControlDevice
    {
        private readonly string baseUrl;

        public override string UpdateUrl => baseUrl + "/keyboard";

        public override Model.LedDataObject GetUpdateModel()
        {
            return Model.LedData("CHROMA_CUSTOM", RazerDriver.ToJaggedArray(LEDs,22,6));
        }

        public KeyboardDevice(string url, ISimpleLed driver)
        {
            baseUrl = url;
            Driver = driver;
            DeviceType = DeviceTypes.Keyboard;
            Name = "Keyboard";
            ProductImage = RazerDriver.GetImage("Keyboard");
            Has2DSupport = true;
            GridHeight = 6;
            GridWidth = 22;
            LEDs = new LedUnit[132];

            int xPos = 0;
            int yPos = 0;

            for (int i = 0; i < 132; i++)
            {
                if (xPos > 21)
                {
                    xPos = 0;
                    yPos++;
                }
                LEDs[i] = new LedUnit
                {
                    LEDName = "LED " + i.ToString(),
                    Data = new PositionalLEDData()
                    {
                        X = xPos,
                        Y = yPos,
                        LEDNumber = i
                    }
                };
                xPos++;
            }
        }
    }
}
