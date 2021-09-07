namespace ElevatorLockdown
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using ElevatorLockdown.Configs;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using MEC;
    using Broadcast = Broadcast;
    using Random = UnityEngine.Random;

    public class Methods
    {
        private readonly Plugin _plugin;
        public Methods(Plugin plugin) => this._plugin = plugin;

        public void EnableElevators()
        {
            if (_plugin.Config.LockdownEndGlobal != null && !string.IsNullOrEmpty(_plugin.Config.LockdownEndGlobal.Text) && _plugin.Config.LockdownEndGlobal.Duration > 0)
                SendBroadcast(true);
            
            if (_plugin.Config.LockdownEndCassie != null && !string.IsNullOrEmpty(_plugin.Config.LockdownEndCassie.Message))
                SendCassieMessage(true);
            
            _plugin.DisabledElevators.Clear();
        }
        public void DisableElevators()
        {
            foreach ((ElevatorType type, int chance) in _plugin.Config.AffectedElevators)
            {
                if (Random.Range(0, 101) <= chance)
                    _plugin.DisabledElevators.Add(type);
            }

            if (_plugin.Config.LockdownStartGlobal != null && !string.IsNullOrEmpty(_plugin.Config.LockdownStartGlobal.Text) && _plugin.Config.LockdownStartGlobal.Duration > 0)
                SendBroadcast(false);

            if (_plugin.Config.LockdownStartCassie != null && !string.IsNullOrEmpty(_plugin.Config.LockdownStartCassie.Message))
                SendCassieMessage(false);
        }

        public void SendBroadcast(bool enabled)
        {
            BroadcastMessage message = enabled ? _plugin.Config.LockdownEndGlobal : _plugin.Config.LockdownStartGlobal;
            string text = message.Text.Replace("%elevators%", GetElevatorNames());
            
            Map.Broadcast((ushort)message.Duration, text, Broadcast.BroadcastFlags.Normal, false);
        }

        public void SendCassieMessage(bool enabled)
        {
            CassieMessage message = enabled ? _plugin.Config.LockdownEndCassie : _plugin.Config.LockdownStartCassie;

            Cassie.Message(message.Message.Replace("%count%", _plugin.DisabledElevators.Count.ToString()));
        }

        public string GetElevatorNames()
        {
            string elevatorNames = string.Empty;
            foreach (ElevatorType type in _plugin.DisabledElevators)
                elevatorNames += $"{Regex.Replace(type.ToString(), @"(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", " $1").Trim()}, ";
            return elevatorNames.Remove(elevatorNames.LastIndexOf(",", StringComparison.Ordinal));
        }

        public bool ValidateLoopConfig() => _plugin.Config.RepeatFrequency != null && !(_plugin.Config.RepeatFrequency.Minimum < 1);

        internal IEnumerator<float> ElevatorLockdown()
        {
            yield return Timing.WaitForSeconds(Random.Range(_plugin.Config.InitialDelay.Minimum, _plugin.Config.InitialDelay.Maximum + 1));

            for (;;)
            {
                DisableElevators();

                yield return Timing.WaitForSeconds(Random.Range(_plugin.Config.LockdownPeriod.Minimum, _plugin.Config.LockdownPeriod.Maximum + 1));

                EnableElevators();

                yield return Timing.WaitForSeconds(Random.Range(_plugin.Config.RepeatFrequency.Minimum, _plugin.Config.RepeatFrequency.Maximum + 1));
            }
        }
    }
}