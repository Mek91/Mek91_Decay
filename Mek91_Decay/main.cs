using Rocket.Core.Plugins;
using Rocket.Core.Logging;
using System;
using System.Linq;
using Mek91_Decay.configs;
using Mek91_Decay.func;
using Rocket.Core.Commands;
using System.Threading;
using SDG.Unturned;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Mek91_Decay.func.getStructure;
using System.Collections;
using Rocket.Core.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using Steamworks;
using static SDG.Provider.SteamGetInventoryResponse;

namespace Mek91_Decay
{
    public class main : RocketPlugin<DecayConfig>
    {
        private CSteamID damageOrigin;

        protected override void Load()
        {
            Logger.LogWarning(new logASCIIArt().GetAsciiArt);
            Logger.Log($"{Name} {Assembly.GetName().Version} launching!");

            Level.onLevelLoaded += OnServerInitialized;
        }

        private void OnServerInitialized(int level)
        {
            new whitelist_NoDecay(Configuration.Instance);
            StartCoroutine(Count());
            Logger.Log("The server is fully loaded and Mek91_Decay is running!");
        }

        protected override void Unload()
        {
            StopCoroutine(Count());
            Logger.Log($"{Name} has been unloaded!");
        }

        private IEnumerator Count()
        {
            while (true)
            {
                yield return Run_DecayAsync();
                yield return new WaitForSeconds(Configuration.Instance.DecayIntervalMinute * 60f);
            }
        }

        private async Task Run_DecayAsync()
        {
            try
            {
                Logger.Log("Decay and Regeneration Launched " + DateTime.Now);
                await DecayAsync();
                await DecayRenewalAsync();
                Logger.Log("Decay and Regeneration Finished " + DateTime.Now);
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                Logger.LogWarning(e.Message.ToString());
            }
        }

        private async Task ApplyDecayAsync<T>(List<T> items, Func<T, float> getHealth, Func<T, ulong> getItemID, Action<T> applyDamage, Action<T> DestroyItem)
        {
            var decayAmount = Configuration.Instance.DecayAmount;

            foreach (var item in items)
            {
                var currentHealth = getHealth(item);

                if (new whitelist_NoDecay(Configuration.Instance).check(getItemID(item)))
                {
                    continue;
                }

                if (currentHealth <= 0)
                {
                    DestroyItem(item);
                    continue;
                }

                await Task.Yield();

                ThreadUtil.assertIsGameThread();

                applyDamage(item);
            }
        }

        private async Task DecayAsync()
        {
            var getBarricadesInstance = new getBarricades();
            if (getBarricadesInstance == null)
            {
                Logger.LogError("getBarricades instance is null");
                return;
            }

            var barricades = getBarricadesInstance.GetAllBarricades(false);
            if (barricades == null)
            {
                Logger.LogError("GetAllBarricades returned null");
                return;
            }

            await ApplyDecayAsync(
                barricades,
                b => b.Barricade.asset.health,
                b => b.Barricade.asset.id,
                b => BarricadeManager.damage(b.Barricade.model.transform, Configuration.Instance.DecayAmount, 1, false, damageOrigin, EDamageOrigin.Unknown),
                b => BarricadeManager.Destroy(b.Barricade.model)
            );

            await Decay2Async();
        }

        private async Task Decay2Async()
        {
            var structures = new getStructure().GetAllStructures();

            await ApplyDecayAsync(
                structures,
                s => s.Structure.asset.health,
                s => s.Structure.asset.id,
                s => StructureManager.damage(s.Structure.model.transform, s.Position, Configuration.Instance.DecayAmount, 1, false, damageOrigin, EDamageOrigin.Unknown),
                s => StructureManager.Destroy(s.Structure.model)
            );
        }
        /* ------------------------------------------------------------------------------------------------------------------- */
        private async Task ApplyHealAsync<T>(List<T> items, Func<T, ulong> getItemID, Func<T, Vector3> getPosition, Func<T, float> getRange, Action<T> applyHeal, Vector3 generatorCoord)
        {
            var healCount = Configuration.Instance.HealCount;

            foreach (var item in items)
            {
                var position = getPosition(item);
                var range = getRange(item);
                float distance = distance3D.calc(generatorCoord, position);

                if (new whitelist_NoDecay(Configuration.Instance).check(getItemID(item)))
                {
                    continue;
                }

                if (distance > range)
                {
                    continue;
                }

                await Task.Yield();
                ThreadUtil.assertIsGameThread();

                applyHeal(item);
            }
        }

        private async Task DecayRenewalAsync()
        {
            var generators = new getBarricades().GetAllBarricades(true);

            foreach (var generator in generators)
            {
                var cBarricade = generator.Barricade;
                var interactable = cBarricade.interactable as InteractableGenerator;

                if (interactable == null)
                {
                    Logger.LogWarning($"This \"{generator.instanceID}\" barricade is not a generator.");
                    continue;
                }

                float range = interactable.wirerange;
                bool heal = interactable.isPowered && interactable.fuel > 0;

                if (heal)
                {
                    await ApplyHealAsync(
                        new List<getBarricades.BarricadeInfo> { generator },
                        b => b.Barricade.asset.id,
                        b => b.Position,
                        b => range,
                        b => BarricadeManager.repair(b.Barricade.model.transform, Configuration.Instance.HealCount, 1),
                        generator.Position
                    );

                    await Decay_Renewal2Async(range, generator.Position);
                }
            }
        }

        private async Task Decay_Renewal2Async(float range, Vector3 generatorCoord)
        {
            var barricades = new getBarricades().GetAllBarricades(false);
            var structures = new getStructure().GetAllStructures();

            await ApplyHealAsync(
                barricades,
                b => b.Barricade.asset.id,
                b => b.Position,
                _ => range,
                b => BarricadeManager.repair(b.Barricade.model.transform, Configuration.Instance.HealCount, 1),
                generatorCoord
            );

            await ApplyHealAsync(
                structures,
                s => s.Structure.asset.id,
                s => s.Position,
                _ => range,
                s => StructureManager.repair(s.Structure.model.transform, Configuration.Instance.HealCount, 1),
                generatorCoord
            );
        }
    }
}