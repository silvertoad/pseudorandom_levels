using System;

public struct Point<TType>
{
    public TType X;
    public TType Y;

    public Point (TType _x, TType _y)
    {
        X = _x;
        Y = _y;
    }

    public override string ToString ()
    {
        return string.Format ("Point: x: {0}, y: {1}.", X, Y);
    }

    public override bool Equals (object _other)
    {
        if (_other is Point<TType>) {
            Point<TType> c = (Point<TType>)_other;
            return X.Equals (c.X) && Y.Equals (c.Y);
        } else {
            return false;
        }
    }

    public static bool operator== (Point<TType> _lhs, Point<TType> _rhs)
    {
        return _lhs.Equals (_rhs);
    }

    public static bool operator!= (Point<TType> _lhs, Point<TType> _rhs)
    {
        return !_lhs.Equals (_rhs);
    }

    public override int GetHashCode ()
    {
        return X.GetHashCode () ^ Y.GetHashCode ();
    }
}