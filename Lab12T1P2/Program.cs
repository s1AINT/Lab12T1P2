using System;

class Program
{
    static void Main()
    {
        int mod = 23;
        Point basePoint = new Point(17, 25);

        Console.WriteLine($"Елiптична крива: y^2 = (x^3 + x + 1) mod {mod}");
        Console.WriteLine($"Базова точка G = ({basePoint.X}, {basePoint.Y})");

        int order = FindOrder(basePoint, mod);
        Console.WriteLine($"Порядок базової точки: {(order == -1 ? "нескінченний" : order.ToString())}");
    }

    static int FindOrder(Point basePoint, int mod)
    {
        Point current = basePoint;
        int order = 1;

        while (!current.IsNeutralElement())
        {
            current = AddPoints(current, basePoint, mod);
            order++;

            if (order > mod)
            {
                // Якщо порядок перевищує порядок поля, щось пішло не так
                return -1;
            }
        }

        return order;
    }

    static Point AddPoints(Point point1, Point point2, int mod)
    {
        if (point1.IsNeutralElement())
        {
            return point2;
        }
        if (point2.IsNeutralElement())
        {
            return point1;
        }

        int slope;
        if (point1.Equals(point2))
        {
            slope = ((3 * point1.X * point1.X) % mod + 1) * ModInverse(2 * point1.Y, mod) % mod;
        }
        else
        {
            slope = ((point2.Y - point1.Y) % mod + mod) * ModInverse((point2.X - point1.X + mod) % mod, mod) % mod;
        }

        int x3 = (slope * slope - point1.X - point2.X + mod + mod) % mod;
        int y3 = (slope * (point1.X - x3) - point1.Y + mod) % mod;

        return new Point(x3, y3);
    }

    static int ModInverse(int a, int m)
    {
        for (int i = 1; i < m; i++)
        {
            if ((a * i) % m == 1)
            {
                return i;
            }
        }

        return -1;
    }
}

class Point
{
    public int X { get; }
    public int Y { get; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public bool IsNeutralElement()
    {
        return X == 0 && Y == 0;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Point other = (Point)obj;
        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}
