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
    public class Lock : ICommand
    {
        public string Command { get; } = "elock";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Locks specified elevators.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get((CommandSender)sender);
            if (player == null)
            {
                response = "You must be authenticated on the server to run this command.";
                return false;
            }

            if (!player.CheckPermission("el.lock"))
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

                if (Plugin.Instance.DisabledElevators.Contains(type))
                {
                    response += $"{type} is already locked.\n";
                    continue;
                }
                
                types.Add(type);
                response += $"{type} has been locked.\n";
            }

            if (types.Count == 0)
            {
                response += "No elevators were able to be locked.";
                return false;
            }
            
            foreach (ElevatorType type in types)
                Plugin.Instance.DisabledElevators.Add(type);
            
            ListPool<ElevatorType>.Shared.Return(types);

            return true;
        }
    }
}