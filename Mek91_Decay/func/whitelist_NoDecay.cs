using Mek91_Decay.configs;

namespace Mek91_Decay.func
{
    public class whitelist_NoDecay
    {
        private DecayConfig _config;

        public whitelist_NoDecay(DecayConfig config)
        {
            _config = config;
        }

        public bool check(ulong itemID)
        {
            var whitelist_NoDecay = _config.Whitelist_NoDecay;

            foreach (var whitelist in whitelist_NoDecay)
            {
                if (whitelist == itemID)
                    return true;
            }

            return false;
        }
    }
}