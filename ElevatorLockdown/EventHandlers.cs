namespace ElevatorLockdown
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.Events.EventArgs;
    using MEC;
    using UnityEngine;

    public class EventHandlers
    {
        private readonly Plugin _plugin;
        private CoroutineHandle _coroutine;
        public EventHandlers(Plugin plugin) => this._plugin = plugin;

        public void OnInteractingElevator(InteractingElevatorEventArgs ev)
        {
            if (_plugin.DisabledElevators.Contains(ev.Lift.Type()))
            {
                ev.IsAllowed = false;
                if (_plugin.Config.ElevatorDisabled != null && !string.IsNullOrEmpty(_plugin.Config.ElevatorDisabled.Text) && _plugin.Config.ElevatorDisabled.Duration > 0)
                {
                    ev.Player.ShowHint(_plugin.Config.ElevatorDisabled.Text.Replace("%elevator%", "This elevator"), _plugin.Config.ElevatorDisabled.Duration);
                }
            }
        }

        public void OnWaitingForPlayers()
        {
            if (_coroutine.IsValid)
                Timing.KillCoroutines(_coroutine);
        }

        public void OnRoundStarted()
        {
            _coroutine = Timing.RunCoroutine(_plugin.Methods.ElevatorLockdown());
        }
    }
}