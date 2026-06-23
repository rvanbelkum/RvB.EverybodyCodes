using System.Numerics;

namespace RvB.EC.Shared;

public readonly struct Matrix<T> where T : struct, INumber<T> {
    // Layed out per row: R0C0 R0C1 R0C2 R0C3 R1C0 ...
    readonly T[] _cells = new T[16];

    public Matrix() { }

    private T this[int row, int col] {
        get => _cells[row * 4 + col];
        set => _cells[row * 4 + col] = value;
    }

    public Matrix(Point3D<T> v1, Point3D<T> v2, Point3D<T> v3) {
        this[0, 0] = v1.X;
        this[1, 0] = v1.Y;
        this[2, 0] = v1.Z;
        this[3, 0] = T.Zero;

        this[0, 1] = v2.X;
        this[1, 1] = v2.Y;
        this[2, 1] = v2.Z;
        this[3, 1] = T.Zero;

        this[0, 2] = v3.X;
        this[1, 2] = v3.Y;
        this[2, 2] = v3.Z;
        this[3, 2] = T.Zero;

        this[0, 3] = T.Zero;
        this[1, 3] = T.Zero;
        this[2, 3] = T.Zero;
        this[3, 3] = T.One;
    }

    private Matrix(T[] cells) {
        _cells = cells;
    }

    public static Point3D<T> operator *(Matrix<T> m, Point3D<T> p)
        => new(
            m[0, 0] * p.X + m[0, 1] * p.Y + m[0, 2] * p.Z + m[0, 3],
            m[1, 0] * p.X + m[1, 1] * p.Y + m[1, 2] * p.Z + m[1, 3],
            m[2, 0] * p.X + m[2, 1] * p.Y + m[2, 2] * p.Z + m[2, 3]
            );

    public static Matrix<T> operator *(Matrix<T> m1, Matrix<T> m2)
        => new([
            m1[0, 0] * m2[0, 0] + m1[0, 1] * m2[1, 0] + m1[0, 2] * m2[2, 0] + m1[0, 3] * m2[3, 0],
            m1[0, 0] * m2[0, 1] + m1[0, 1] * m2[1, 1] + m1[0, 2] * m2[2, 1] + m1[0, 3] * m2[3, 1],
            m1[0, 0] * m2[0, 2] + m1[0, 1] * m2[1, 2] + m1[0, 2] * m2[2, 2] + m1[0, 3] * m2[3, 2],
            m1[0, 0] * m2[0, 3] + m1[0, 1] * m2[1, 3] + m1[0, 2] * m2[2, 3] + m1[0, 3] * m2[3, 3],

            m1[1, 0] * m2[0, 0] + m1[1, 1] * m2[1, 0] + m1[1, 2] * m2[2, 0] + m1[1, 3] * m2[3, 0],
            m1[1, 0] * m2[0, 1] + m1[1, 1] * m2[1, 1] + m1[1, 2] * m2[2, 1] + m1[1, 3] * m2[3, 1],
            m1[1, 0] * m2[0, 2] + m1[1, 1] * m2[1, 2] + m1[1, 2] * m2[2, 2] + m1[1, 3] * m2[3, 2],
            m1[1, 0] * m2[0, 3] + m1[1, 1] * m2[1, 3] + m1[1, 2] * m2[2, 3] + m1[1, 3] * m2[3, 3],

            m1[2, 0] * m2[0, 0] + m1[2, 1] * m2[1, 0] + m1[2, 2] * m2[2, 0] + m1[2, 3] * m2[3, 0],
            m1[2, 0] * m2[0, 1] + m1[2, 1] * m2[1, 1] + m1[2, 2] * m2[2, 1] + m1[2, 3] * m2[3, 1],
            m1[2, 0] * m2[0, 2] + m1[2, 1] * m2[1, 2] + m1[2, 2] * m2[2, 2] + m1[2, 3] * m2[3, 2],
            m1[2, 0] * m2[0, 3] + m1[2, 1] * m2[1, 3] + m1[2, 2] * m2[2, 3] + m1[2, 3] * m2[3, 3],

            m1[3, 0] * m2[0, 0] + m1[3, 1] * m2[1, 0] + m1[3, 2] * m2[2, 0] + m1[3, 3] * m2[3, 0],
            m1[3, 0] * m2[0, 1] + m1[3, 1] * m2[1, 1] + m1[3, 2] * m2[2, 1] + m1[3, 3] * m2[3, 1],
            m1[3, 0] * m2[0, 2] + m1[3, 1] * m2[1, 2] + m1[3, 2] * m2[2, 2] + m1[3, 3] * m2[3, 2],
            m1[3, 0] * m2[0, 3] + m1[3, 1] * m2[1, 3] + m1[3, 2] * m2[2, 3] + m1[3, 3] * m2[3, 3]
            ]);
}
