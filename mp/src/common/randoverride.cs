namespace SourceSharp.mp.src.common
{
    public class randoverride
    {
        public static void srand(uint uiInput)
        {
        }

        public static int rand()
        {
            return random.RandomInt(0, platform.VALVE_RAND_MAX);
        }
    }
}
