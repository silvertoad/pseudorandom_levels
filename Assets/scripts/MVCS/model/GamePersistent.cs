namespace mvscs.model
{
    public class GamePersistent
    {
        public int Seed { get; private set; }

        public void SetSeed (int _seed)
        {
            Seed = _seed;
        }
    }
}