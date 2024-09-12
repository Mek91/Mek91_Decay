using Rocket.API;
using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using UnityEngine;
using SDG.Unturned;
using Logger = Rocket.Core.Logging.Logger;
using Rocket.Unturned.Player;

namespace Mek91_Decay.func
{
    internal class goInstance : IRocketCommand
    {
        public string Name => "go_instance";
        public string Help => "Allows you to navigate to a specific location by InstanceID.";
        public string Syntax => "/go_instance <baric/struc> <instanceID>";
        public List<string> Aliases => new List<string>() { "goinst" };
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public List<string> Permissions => new List<string>() { "go.instance" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length != 2)
            {
                Logger.LogError("Invalid syntax! Use the format: " + Syntax);
                return;
            }

            string type = command[0].ToLower();
            if (!ulong.TryParse(command[1], out ulong instanceID))
            {
                Logger.LogError("Invalid instanceID!");
                return;
            }

            var player = (UnturnedPlayer)caller;
            Logger.Log($"Admin {player.CharacterName} is trying to teleport to instanceID: {instanceID}.");

            if (type == "baric")
            {
                var barricades = new getBarricades().GetAllBarricades(false);
                var targetBarricade = barricades.Find(b => b.instanceID == instanceID);

                if (targetBarricade != null)
                {
                    Vector3 position = targetBarricade.Position;
                    player.Player.teleportToLocationUnsafe(position, player.Player.transform.rotation.eulerAngles.y);
                    Logger.Log($"Player {player.CharacterName} successfully teleported to {position}.");
                }
                else
                {
                    Logger.LogError("No barricade found with the specified InstanceID.");
                }
            }
            else if (type == "struc")
            {
                var structures = new getStructure().GetAllStructures();
                var targetStructure = structures.Find(s => s.instanceID == instanceID);

                if (targetStructure != null)
                {
                    Vector3 position = targetStructure.Position;
                    player.Player.teleportToLocationUnsafe(position, player.Player.transform.rotation.eulerAngles.y);
                    Logger.Log($"Player {player.CharacterName} successfully teleported to {position}.");
                }
                else
                {
                    Logger.LogError("No structure found with the specified InstanceID.");
                }
            }
            else
            {
                Logger.LogError("Invalid type specified! Use 'baric' for barricades or 'struc' for structures.");
            }
        }
    }
}