using System;

public struct Point<TType>
{
    public TType X;
    public TType Y;

    public override string ToString ()
    {
        return string.Format ("Point: x: {0}, y: {1}.", X, Y);
    }
}