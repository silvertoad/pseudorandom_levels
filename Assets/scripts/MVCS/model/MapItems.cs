namespace mvscs.model
{
    public class MapItem
    {
        public MapItem (MapItemDef _def)
        {
            Def = _def;
        }

        public MapItemDef Def { get; private set; }

        public Point<int> Position { get; set; }

        public void Release ()
        {
            throw new System.NotImplementedException ();
        }
    }
}