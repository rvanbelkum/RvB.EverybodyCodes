using RvB.EC.Shared;
using RvB.Grids;

namespace RvB.EC.Shared;

public static class GridExtensions {
    extension<T>(Grid<T> grid) {
        public Grid<T> Stretch(int widthFactor, int heightFactor)
            => grid.Stretch(widthFactor, heightFactor, (v, _, _) => v);

        public Grid<T> Stretch(int widthFactor, int heightFactor, Func<T?, int, int, T?> map) {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(widthFactor, nameof(widthFactor));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(heightFactor, nameof(heightFactor));

            var stretched = new Grid<T>(grid.Width * widthFactor, grid.Height * heightFactor, grid.TopLeftIndex);
            var sRow = 0;
            foreach (var row in grid.Rows) {
                var sCol = 0;
                foreach (var (value, _) in row) {
                    for (var dr = 0; dr < heightFactor; dr++) {
                        for (var dc = 0; dc < widthFactor; dc++) {
                            stretched[sCol + dc, sRow + dr] = map(value, dc, dr);
                        }
                    }
                    sCol += widthFactor;
                }
                sRow += heightFactor;
            }
            return stretched;
        }
    }
}
