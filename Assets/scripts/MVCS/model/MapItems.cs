namespace mvscs.model
{
    public struct MapItem
    {
        public MapItem (MapItemDef _def)
        {
            Def = _def;
            Position = new Point<int> ();
        }

        public MapItemDef Def;

        public Point<int> Position;

        public void Release ()
        {
            throw new System.NotImplementedException ();
        }

        public override bool Equals (object _other)
        {
            if (_other is MapItem) {
                var other = (MapItem)_other;
                return Def.Equals (other.Def) && Position == other.Position;
            }
            return false;
        }

        public override string ToString ()
        {
            return string.Format ("[MapItem] Def = {0}, Position = {1}.", Def, Position);
        }
    }
}