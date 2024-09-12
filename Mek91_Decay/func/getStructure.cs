using SDG.Unturned;
using UnityEngine;
using System.Collections.Generic;

namespace Mek91_Decay.func
{
    internal class getStructure
    {
        public class Position
        {
            public float x { get; set; }
            public float y { get; set; }
            public float z { get; set; }
        }

        public class StructureInfo
        {
            public ulong OwnerID { get; set; }
            public Vector3 Position { get; set; }
            public ushort ItemID { get; set; }
            public string ItemName { get; set; }
            public ulong instanceID { get; set; }
            public StructureDrop Structure { get; set; }
        }

        public List<StructureInfo> GetAllStructures()
        {
            List<StructureInfo> structuresList = new List<StructureInfo>();

            foreach (StructureRegion region in StructureManager.regions)
            {
                foreach (StructureDrop structure in region.drops)
                {
                    ulong ownerID = structure.GetServersideData().owner;

                    StructureInfo info = new StructureInfo
                    {
                        OwnerID = ownerID,
                        Position = new Vector3(
                            structure.model.position.x,
                            structure.model.position.y,
                            structure.model.position.z
                        ),
                        ItemID = structure.asset.id,
                        ItemName = structure.asset.itemName.ToLower(),
                        instanceID = structure.instanceID,
                        Structure = structure
                    };

                    structuresList.Add(info);
                }
            }

            return structuresList;
        }
    }
}