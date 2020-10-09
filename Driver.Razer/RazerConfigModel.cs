using Newtonsoft.Json;
using SimpleLed;

namespace Driver.Razer
{
    public class RazerConfigModel : SLSConfigData
    {
        private bool showOnlyConnected = true;
        public bool ShowOnlyConnected
        {
            get => showOnlyConnected;
            set
            {
                SetProperty(ref showOnlyConnected, value);
                DataIsDirty = true;
            }
        }

        private ControlDevice controlDevice;

        [JsonIgnore]
        public ControlDevice CurrentControlDevice
        {
            get => controlDevice;
            set => SetProperty(ref controlDevice, value);
        }
    }
}