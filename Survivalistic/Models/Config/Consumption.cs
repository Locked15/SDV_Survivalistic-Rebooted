namespace Survivalistic_Rebooted.Models.Config
{
    public class Consumption
    {
        public float Hunger { get; set; }

        public float Thirst { get; set; }

        public Consumption(float hunger, float thirst)
        {
            Hunger = hunger;
            Thirst = thirst;
        }

        public static implicit operator Consumption((float, float) values)
        {
            return new(values.Item1, values.Item2);
        } 
    }
}
