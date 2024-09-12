using Rocket.API;
using Rocket.Core.Commands;
using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mek91_Decay.func
{
    internal class debugCommand : IRocketCommand
    {
        public string Name => "decay_debug";
        public string Help => "This command can be executed from the console.";
        public string Syntax => "/decay_debug <baric/struc> <true/false>";
        public List<string> Aliases => new List<string>();
        public AllowedCaller AllowedCaller => AllowedCaller.Console;
        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length < 2)
            {
                Logger.LogError("Invalid syntax! Use the format: " + Syntax);
                return;
            }

            string type = command[0].ToLower();
            bool filter = command[1].ToLower() == "true";

            if (type == "baric")
            {
                var getBarricades = new getBarricades();
                var barricadesList = getBarricades.GetAllBarricades(filter);

                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                string json = JsonConvert.SerializeObject(barricadesList, settings);
                Logger.Log("Barricades list in JSON format: " + json);
            }
            else if (type == "struc")
            {
                var getStruc = new getStructure();
                var strucList = getStruc.GetAllStructures();

                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                string json = JsonConvert.SerializeObject(strucList, settings);
                Logger.Log("Structures list in JSON format: " + json);
            }
            else
            {
                Logger.LogError("Invalid type specified! Use 'baric' or 'struc'.");
            }
        }
    }
}