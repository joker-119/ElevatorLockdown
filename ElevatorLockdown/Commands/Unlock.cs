using System;
using CommandSystem;

namespace ElevatorLockdown.Commands
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using NorthwoodLib.Pools;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Unlock : ICommand
    {
        public string Command { get; } = "eunlock";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Unlocks specific elevators.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get((CommandSender)sender);
            if (player == null)
            {
                response = "You must be authenticated on the server to run this command.";
                return false;
            }

            if (!player.CheckPermission("el.unlock"))
            {
                response = "Permission denied.";
                return false;
            }
            
            List<ElevatorType> types = ListPool<ElevatorType>.Shared.Rent();
            response = string.Empty;

            foreach (string s in arguments)
            {
                if (!Enum.TryParse(s, out ElevatorType type))
                {
                    response += $"{s} is not a valid elevator type.\n";
                    continue;
                }

                if (!Plugin.Instance.DisabledElevators.Contains(type))
                {
                    response += $"{type} is not locked.\n";
                    continue;
                }
                
                types.Add(type);
                response += $"{type} has been unlocked.\n";
            }

            if (types.Count == 0)
            {
                response += "No elevators were able to be unlocked.";
                return false;
            }
            
            foreach (ElevatorType type in types)
                Plugin.Instance.DisabledElevators.Remove(type);
            
            ListPool<ElevatorType>.Shared.Return(types);

            return true;
        }
    }
}