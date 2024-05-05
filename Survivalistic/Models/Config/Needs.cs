namespace Survivalistic_Rebooted.Models.Config
{
    public class Needs
    {
        public float Hunger { get; set; }

        public float Thirst { get; set; }

        public Needs(float hunger, float thirst)
        {
            Hunger = hunger;
            Thirst = thirst;
        }

        public static implicit operator Needs((float, float) values)
        {
            return new(values.Item1, values.Item2);
        }
    }
}
