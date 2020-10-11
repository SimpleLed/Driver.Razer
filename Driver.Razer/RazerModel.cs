using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
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
                param = ConvertToIntArray(leds)
            };
        }

        public class LedDataObject
        {
            public string effect { get; set; }
            public int[] param { get; set; }
        }

        private static int[] ConvertToIntArray(ControlDevice.LedUnit[] leds) => leds.Select(x => RazerDriver.ToBgr(x.Color)).ToArray();
    }
}
