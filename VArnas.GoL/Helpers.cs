using VArnas.GoL.Extensions;

namespace VArnas.GoL;

public static class Helpers
{
    public static bool[,] LoadInitialState(string filename)
    {
        var input = File.ReadAllText(filename);

        var lines = input
            .Split('\r', '\n')
            .Where(l => l != string.Empty)
            .ToArray();
 
        var gridWidth = lines.First().Length;
        var gridHeight = lines.Length;
        var grid = new bool[gridWidth, gridHeight];

        if(!lines.Select(l => l.Length).All(gridWidth.Equals))
            throw new Exception("Grid must not be jagged");
        
        grid.Iterate((x, y) => grid[y, x] = lines[y][x] is '#');
        return grid;
    }
}