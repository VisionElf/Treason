using UnityEngine;

public class Point
{
    public int I;
    public int J;

    public Point(int i, int j)
    {
        I = i;
        J = j;
    }

    public Point(float i, float j)
    {
        I = Mathf.RoundToInt(i);
        J = Mathf.RoundToInt(j);
    }

    public override string ToString()
    {
        return $"({I}x{J})";
    }

    public static Point operator -(Point a, Point b)
    {
        return new Point(a.I - b.I, a.J - b.J);
    }
}