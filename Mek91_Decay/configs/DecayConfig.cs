using Rocket.API;
using System.Collections.Generic;

namespace Mek91_Decay.configs
{
    public class DecayConfig : IRocketPluginConfiguration
    {
        public float HealCount;

        public float DecayIntervalMinute;

        public float DecayAmount;

        public List<ulong> Whitelist_NoDecay;

        public void LoadDefaults()
        {
            DecayIntervalMinute = 20f;
            HealCount = 6f;
            DecayAmount = 5f;

            Whitelist_NoDecay = new List<ulong>()
            {
                1241,
            };
        }
    }
}