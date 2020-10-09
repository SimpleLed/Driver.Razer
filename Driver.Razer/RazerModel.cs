using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using System.Threading.Tasks;
using SimpleLed;

namespace Driver.Razer
{
    public class Model
    {
        public static LedDataObject LedData(string effectType, ControlDevice.LedUnit[] leds)
        {
            return new LedDataObject
            {
                effect = effectType,
                param = ConvertToIntArray(leds)
            };
        }

        private static int[] ConvertToIntArray(ControlDevice.LedUnit[] leds)
        {
            int[] colors = new int[leds.Length];
            for (int i = 0; i < leds.Length; i++)
            {
                colors[i] = RazerDriver.ToBgr(leds[i].Color);
            }

            return colors;
        }

        public class LedDataObject
        {
            public string effect { get; set; }
            public int[] param { get; set; }
        }
    }
}
