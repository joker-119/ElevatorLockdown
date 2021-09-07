namespace ElevatorLockdown.Configs
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Enums;
    using Exiled.API.Interfaces;

    public class Config : IConfig
    {
        [Description("Whether or not this plugin is enabled.")]
        public bool IsEnabled { get; set; }

        public List<LockdownChance> AffectedElevators { get; set; } = new List<LockdownChance>
        {
            new LockdownChance
            {
                Type = ElevatorType.GateA,
                Chance = 100,
            },
            new LockdownChance
            {
                Type = ElevatorType.GateB,
                Chance = 90,
            },
            new LockdownChance
            {
                Type = ElevatorType.Scp049,
                Chance = 50,
            },
        };

        public Delay InitialDelay { get; set; } = new Delay
        {
            Minimum = 220f,
            Maximum = 380f,
        };

        public Delay LockdownPeriod { get; set; } = new Delay
        {
            Minimum = 20f,
            Maximum = 90f,
        };

        public Delay RepeatFrequency { get; set; } = new Delay
        {
            Minimum = 120f,
            Maximum = 240f,
        };

        public HintMessage ElevatorDisabled { get; set; } = new HintMessage
        {
            Text = "%elevator% is currently disabled.",
            Duration = 3,
        };

        public BroadcastMessage LockdownStartGlobal { get; set; } = new BroadcastMessage
        {
            Text =
                "<color=red>Critical system failure detected in</color><color=green> %elevators% </color><color=red>elevator</color>",
            Duration = 5,
        };

        public BroadcastMessage LockdownEndGlobal { get; set; } = new BroadcastMessage
        {
            Text = "<color=green>Elevator system now operational.",
            Duration = 5,
        };

        public CassieMessage LockdownStartCassie { get; set; } = new CassieMessage
        {
            Message = ".g5 .g6 Critical elevator system failure detected. %count% elevators affected.",
        };

        public CassieMessage LockdownEndCassie { get; set; } = new CassieMessage
        {
            Message = "Elevator system now fully operational.",
        };
    }
}