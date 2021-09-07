using System;
using Exiled.API.Features;
using MapEvents = Exiled.Events.Handlers.Map;
using PlayerEvents = Exiled.Events.Handlers.Player;
using Scp049Events = Exiled.Events.Handlers.Scp049;
using Scp079Events = Exiled.Events.Handlers.Scp079;
using Scp096Events = Exiled.Events.Handlers.Scp096;
using Scp106Events = Exiled.Events.Handlers.Scp106;
using Scp914Events = Exiled.Events.Handlers.Scp914;
using ServerEvents = Exiled.Events.Handlers.Server;
using WarheadEvents = Exiled.Events.Handlers.Warhead;

namespace ElevatorLockdown
{
    using System.Collections.Generic;
    using ElevatorLockdown.Configs;
    using Exiled.API.Enums;
    using NorthwoodLib.Pools;

    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance;
        
        public override string Author { get; } = "Joker119";
        public override string Name { get; } = "ElevatorLockdown";
        public override string Prefix { get; } = "ElevatorLockdown";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 14, 0);

        public EventHandlers EventHandlers { get; private set; }
        public Methods Methods { get; private set; }

        public HashSet<ElevatorType> DisabledElevators { get; private set; }

        public override void OnEnabled()
        {
            Methods = new Methods(this);

            if (!Methods.ValidateLoopConfig())
            {
                Log.Error($"Invalid repeat frequency detected. The minimum delay must be set to a value above 0. This plugin will not be enabled.");
                Methods = null;
                return;
            }

            Instance = this;
            EventHandlers = new EventHandlers(this);
            DisabledElevators = HashSetPool<ElevatorType>.Shared.Rent();

            Exiled.Events.Handlers.Player.InteractingElevator += EventHandlers.OnInteractingElevator;
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnRoundStarted;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.InteractingElevator -= EventHandlers.OnInteractingElevator;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundStarted;
            
            HashSetPool<ElevatorType>.Shared.Return(DisabledElevators);
            EventHandlers = null;
            Methods = null;

            base.OnDisabled();
        }
    }
}