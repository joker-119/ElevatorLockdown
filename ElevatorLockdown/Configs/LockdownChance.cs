namespace ElevatorLockdown.Configs
{
    using Exiled.API.Enums;

    public class LockdownChance
    {
        public ElevatorType Type { get; set; }
        public int Chance { get; set; }

        public void Deconstruct(out ElevatorType type, out int chance)
        {
            type = Type;
            chance = Chance;
        }
    }
}