using stuff.graph.net;

namespace stuff.graph.astar.net;

public static class Heuristic
{
    public static double MaxDXDYDZ(INode a, INode b)
    {
        var aLocation = a.Location;
        var bLocation = b.Location;
        var x = Math.Abs(aLocation.X - bLocation.X);
        var y = Math.Abs(aLocation.Y - bLocation.Y);
        var z = Math.Abs(aLocation.Z - bLocation.Z);
        return Math.Max(x, Math.Max(y, z));
    }

    public static double DiagonalShortCut(INode a, INode b)
    {
        var aLocation = a.Location;
        var bLocation = b.Location;

        var dx = Math.Abs(aLocation.X - bLocation.X);
        var dy = Math.Abs(aLocation.Y - bLocation.Y);
        var dz = Math.Abs(aLocation.Z - bLocation.Z);

        var minXY = Math.Min(dx, dy);
        var minXZ = Math.Min(dx, dz);
        var minYZ = Math.Min(dy, dz);

        var diagonalMoves = Math.Min(minXY, Math.Min(minXZ, minYZ));
        var straightMoves = dx + dy + dz - 2 * diagonalMoves;
        return diagonalMoves * Math.Sqrt(2) + straightMoves;
    }

    public static double Euclidean(INode a, INode b)
    {
        var aLocation = a.Location;
        var bLocation = b.Location;
        return Math.Sqrt(Math.Pow(aLocation.X - bLocation.X, 2) + Math.Pow(aLocation.Y - bLocation.Y, 2) + Math.Pow(aLocation.Z - bLocation.Z, 2));
    }

    public static double Manhatten(INode a, INode b)
    {
        var aLocation = a.Location;
        var bLocation = b.Location;
        return Math.Abs(aLocation.X - bLocation.X) + Math.Abs(aLocation.Y - bLocation.Y) + Math.Abs(aLocation.Z - bLocation.Z);
    }
}
