using System.Runtime.InteropServices;

namespace RvB.EC2024;

public class Quest08 : Quest {
    public override object? Part1(PuzzleInput input) {
        var blockCount = int.Parse(input.AsStringRange());

        var height = (int)Math.Sqrt(blockCount);
        var baseLength = 1 + (height - 1) * 2;
        var targetHeight = height;
        var extra = 0;
        if (height * height < blockCount) {
            targetHeight += 1;
            extra = targetHeight * targetHeight - blockCount;
            baseLength += 2;
        }
        return baseLength * extra;
    }

    public override object? Part2(PuzzleInput input) {
        const int Acolytes = 1111;
        const int BlockCount = 20_240_000;

        var priestCount = int.Parse(input.AsStringRange());

        var blocksNeeded = 0;
        var baseWidth = 0;
        var thickness = 1;
        for (var layer = 1; blocksNeeded < BlockCount; layer += 1) {
            baseWidth = 2 * layer - 1;
            blocksNeeded += baseWidth * thickness;
            thickness = priestCount * thickness % Acolytes;

        }
        return (blocksNeeded - BlockCount) * baseWidth;
    }

    public override object? Part3(PuzzleInput input) {
        const int Acolytes = 10;
        const long BlockCount = 202_400_000L;

        var priestCount = long.Parse(input.AsStringRange());

        var heights = new Dictionary<int, int>();
        var totalBlocks = 0;
        var usedBlocks = 0;
        var thickness = 1;
        for (var layer = 1; usedBlocks < BlockCount; layer++) {
            var baseWidth = 2 * layer - 1;

            totalBlocks += baseWidth * thickness;

            var emptyBlocks = 0;
            for (var l = 0; l < layer - 1; l += 1) {
                ref var height = ref CollectionsMarshal.GetValueRefOrAddDefault(heights, l, out _);
                height += thickness;
                var remove = (int)(priestCount * baseWidth * height % Acolytes);
                emptyBlocks += l != 0 ? 2 * remove : remove;
            }
            heights.AddOrUpdate(layer - 1, thickness);

            thickness = Acolytes + (int)(priestCount * thickness % Acolytes);
            usedBlocks = totalBlocks - emptyBlocks;
        }
        return usedBlocks - BlockCount;
    }
}
