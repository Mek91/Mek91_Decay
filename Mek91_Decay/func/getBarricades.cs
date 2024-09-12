using SDG.Unturned;
using Rocket.Core.Plugins;
using Rocket.Core.Logging;
using UnityEngine;
using System.Collections.Generic;

namespace Mek91_Decay.func
{
    internal class getBarricades
    {
        public class Position
        {
            public float x { get; set; }
            public float y { get; set; }
            public float z { get; set; }
        }

        public class BarricadeInfo
        {
            public ulong OwnerID { get; set; }
            public Vector3 Position { get; set; }
            public ushort ItemID { get; set; }
            public string ItemName { get; set; }
            public ulong instanceID { get; set; }
            public BarricadeDrop Barricade { get; set; }
        }

        public List<BarricadeInfo> GetAllBarricades(bool getGeneratorsOnly)
        {
            List<BarricadeInfo> barricadesList = new List<BarricadeInfo>();

            foreach (BarricadeRegion region in BarricadeManager.regions)
            {
                foreach (BarricadeDrop barricade in region.drops)
                {
                    ulong ownerID = barricade.GetServersideData().owner;

                    if (ownerID == 0)
                    {
                        continue;
                    }

                    BarricadeInfo info = new BarricadeInfo
                    {
                        OwnerID = ownerID,
                        Position = new Vector3(
                            barricade.model.position.x,
                            barricade.model.position.y,
                            barricade.model.position.z
                        ),
                        ItemID = barricade.asset.id,
                        ItemName = barricade.asset.itemName.ToLower(),
                        instanceID = barricade.instanceID,
                        Barricade = barricade
                    };

                    barricadesList.Add(info);
                }
            }

            if (getGeneratorsOnly)
            {
                return barricadesList.FindAll(info => info.ItemName.Contains("generator"));
            }

            return barricadesList;
        }
    }
}