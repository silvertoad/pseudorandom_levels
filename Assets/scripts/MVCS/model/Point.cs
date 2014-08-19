using System;

/// <summary>
/// Struct for X, Y. Note: when use addition operator TType cast to double 
/// </summary>
public struct Point<TType> where TType : IConvertible
{
    public readonly TType X;
    public readonly TType Y;

    public Point (TType _x, TType _y)
    {
        X = _x;
        Y = _y;
    }

    public Point (string _source)
    {
        var coords = _source.Split (';');
        X = (TType)Convert.ChangeType (coords [0], typeof(TType));
        Y = (TType)Convert.ChangeType (coords [1], typeof(TType));
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

    public static Point<TType> operator + (Point<TType> _lhs, Point<TType> _rhs)
    {
        var x = Convert.ToDouble (_lhs.X) + Convert.ToDouble (_rhs.X);
        var y = Convert.ToDouble (_lhs.Y) + Convert.ToDouble (_rhs.Y);

        return new Point<TType> (
            (TType)Convert.ChangeType (x, typeof(TType)),
            (TType)Convert.ChangeType (y, typeof(TType))
        );
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