using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
                param = leds.Select(x => RazerDriver.ToBgr(x.Color)).ToArray()
            };
        }

        public static LedDataObject LedData(string effectType, int[][] leds)
        {
            return new LedDataObject
            {
                effect = effectType,
                param = leds
            };
        }

        public class LedDataObject
        {
            public string effect { get; set; }
            public object param { get; set; }
        }

    }
}
