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
    

    public class MouseDevice : RazerControlDevice
    {
        private readonly string baseUrl;

        public override string UpdateUrl => baseUrl + "/mouse";

        public override Model.LedDataObject GetUpdateModel()
        {
            return Model.LedData("CHROMA_CUSTOM2", RazerDriver.ToJaggedArray(LEDs,7,9));
        }

        public MouseDevice(string url, ISimpleLed driver)
        {
            baseUrl = url;
            Driver = driver;
            DeviceType = DeviceTypes.Mouse;
            Name = "Mouse";
            ProductImage = RazerDriver.GetImage("Mouse");
            Has2DSupport = true;
            GridHeight = 9;
            GridWidth = 7;
            LEDs = new ControlDevice.LedUnit[63];

            int xPos = 0;
            int yPos = 0;
            for (int i = 0; i < 63; i++)
            {
                
                

                if (xPos > 6)
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
