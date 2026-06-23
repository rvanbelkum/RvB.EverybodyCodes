using System.Numerics;
using System.Runtime.CompilerServices;

namespace RvB.EC.Shared;

public readonly struct Point3D<T> : IEquatable<Point3D<T>>, IComparable<Point3D<T>> where T : struct, INumber<T> {
    public static Point3D<T> Zero { get; } = new(T.Zero);

    public readonly T X { get; }
    public readonly T Y { get; }
    public readonly T Z { get; }

    public Point3D() : this(T.Zero, T.Zero, T.Zero) { }

    public Point3D(T n) : this(n, n, n) { }

    public Point3D(Point3D<T> p) : this(p.X, p.Y, p.Z) { }

    public Point3D(T x, T y, T z) {
        X = x;
        Y = y;
        Z = z;
    }

    public T DistanceSquared(Point3D<T> p)
        => (X - p.X) * (X - p.X) + (Y - p.Y) * (Y - p.Y) + (Z - p.Z) * (Z - p.Z);

    public T ManhattanDistance(Point3D<T> p)
        => T.Abs(X - p.X) + T.Abs(Y - p.Y) + T.Abs(Z - p.Z);

    public T Magnitude
        => T.Abs(X) + T.Abs(Y) + T.Abs(Z);

    public void Deconstruct(out T x, out T y, out T z) {
        x = X;
        y = Y;
        z = Z;
    }

    public Point3D<T> Transform(Matrix<T> transform)
        => transform * this;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3D<T> operator +(Point3D<T> p, T n)
        => new(p.X + n, p.Y + n, p.Z + n);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3D<T> operator -(Point3D<T> p, T n)
        => new(p.X - n, p.Y - n, p.Z - n);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3D<T> operator *(Point3D<T> p, T n)
        => new(p.X * n, p.Y * n, p.Z * n);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3D<T> operator /(Point3D<T> p, T n)
        => new(p.X / n, p.Y / n, p.Z / n);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3D<T> operator +(Point3D<T> p, Point3D<T> q)
        => new(p.X + q.X, p.Y + q.Y, p.Z + q.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3D<T> operator -(Point3D<T> p, Point3D<T> q)
        => new(p.X - q.X, p.Y - q.Y, p.Z - q.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Point3D<T> p, Point3D<T> q)
        => p.Equals(q);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Point3D<T> p, Point3D<T> q)
        => !p.Equals(q);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(Point3D<T> p, Point3D<T> q)
        => p.X < q.X || p.Y < q.Y || p.Z < q.Z;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(Point3D<T> p, Point3D<T> q)
        => p.X <= q.X || p.Y <= q.Y || p.Z <= q.Z;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(Point3D<T> p, Point3D<T> q)
        => p.X > q.X || p.Y > q.Y || p.Z > q.Z;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(Point3D<T> p, Point3D<T> q)
        => p.X >= q.X || p.Y >= q.Y || p.Z >= q.Z;

    public static implicit operator Point3D<T>((T X, T Y, T Z) coord)
        => new(coord.X, coord.Y, coord.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3D<T> Min(Point3D<T> p, Point3D<T> q)
        => new(T.Min(p.X, q.X), T.Min(p.Y, q.Y), T.Min(p.Z, q.Z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3D<T> Max(Point3D<T> p, Point3D<T> q)
        => new(T.Max(p.X, q.X), T.Max(p.Y, q.Y), T.Max(p.Z, q.Z));

    public override readonly bool Equals(object? obj) {
        if (obj is Point3D<T> point)
            return Equals(point);
        return false;
    }

    public override readonly int GetHashCode()
        => HashCode.Combine(X, Y, Z);

    public readonly bool Equals(Point3D<T> other)
        => X == other.X && Y == other.Y && Z == other.Z;

    public override string ToString() => $"({X},{Y},{Z})";

    public int CompareTo(Point3D<T> other) {
        if (Equals(other))
            return 0;
        if (this < other)
            return -1;
        return 1;
    }
}
