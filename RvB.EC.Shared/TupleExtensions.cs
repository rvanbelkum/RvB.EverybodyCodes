using System.Numerics;
using System.Runtime.CompilerServices;

namespace RvB.EC.Shared;

public static class TupleExtensions {
    extension<T>((T X, T Y) position) where T : INumber<T> {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T X, T Y) TurnRight() 
            => (-position.Y, position.X);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T X, T Y) TurnLeft()
            => (position.Y, -position.X);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T X, T Y) TurnBack()
            => (-position.X, -position.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T X, T Y) Move((T X, T Y) distance)
            => (position.X + distance.X, position.Y + distance.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T X, T Y) Move(T distanceX, T distanceY)
            => (position.X + distanceX, position.Y + distanceY);
    }
}
