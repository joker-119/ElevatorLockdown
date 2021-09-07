namespace ElevatorLockdown.Configs
{
    public class Delay
    {
        public float Minimum { get; set; }
        public float Maximum { get; set; }
        
        public void Deconstruct(out float min, out float max)
        {
            min = Minimum;
            max = Maximum;
        }
    }
}