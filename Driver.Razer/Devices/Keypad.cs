using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using SimpleLed;

namespace Driver.Razer.Devices
{
   

    public class KeypadDevice : RazerControlDevice
    {
        private readonly string baseUrl;

        public override string UpdateUrl => baseUrl + "/keypad";

        public override Model.LedDataObject GetUpdateModel()
        {
            return Model.LedData("CHROMA_CUSTOM", this.LEDs);
        }

        public KeypadDevice(string url, ISimpleLed driver)
        {
            baseUrl = url;
            Driver = driver;
            DeviceType = DeviceTypes.Keypad;
            Name = "Keypad";
            ProductImage = RazerDriver.GetImage("Keypad");
            Has2DSupport = true;
            GridHeight = 4;
            GridWidth = 5;
            LEDs = new ControlDevice.LedUnit[20];

            int xPos = 0;
            int yPos = 0;
            for (int i = 0; i < 20; i++)
            {
               if (xPos > 4)
                {
                    xPos = 0;
                    yPos++;
                }

                LEDs[i] = new ControlDevice.LedUnit
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

        }
    }
}