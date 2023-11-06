using System.Numerics;
using Raylib_cs;

namespace VArnas.GoL;

public static class Program
{
    private static void Update(bool[,] grid, int [,] neighbourMask)
    {
        var h = grid.GetLength(0);
        var w = grid.GetLength(1);
        for (var y = 0; y < h; ++y)
            for (var x = 0; x < w; ++x)
                neighbourMask[y, x] = CountNeighbours(grid, x, y);

        for (var y = 0; y < h; ++y)
        {
            for (var x = 0; x < w; ++x)
            {
                var n = neighbourMask[y, x];
                if (grid[y, x])
                {
                    if (n is < 2 or > 3)
                        grid[y, x] = false;
                }
                else
                {
                    if (n is 3)
                        grid[y, x] = true;
                }
            }
        }
    }

    private static BigInteger GetGridState(bool[,] grid)
    {
        var h = grid.GetLength(0);
        var w = grid.GetLength(1);

        var state = new BigInteger(0);
        
        for (var y = 0; y < h; ++y)
        {
            for (var x = 0; x < w; ++x)
            {
                if (!grid[y, x]) continue;
                var index = x + y * w;
                state += BigInteger.Pow(2, index);
            }
        }

        return state;
    }
    
    private static int CountNeighbours(bool[,] grid, int x, int y)
    {
        var h = grid.GetLength(0);
        var w = grid.GetLength(1);
        return new[]
            {
                (x - 1, y - 1), (x, y - 1), (x + 1, y - 1),
                (x - 1, y), (x + 1, y),
                (x - 1, y + 1), (x, y + 1), (x + 1, y + 1),
            }
            .Count(p => grid[(h + p.Item2) % h, (w + p.Item1) % w]);
    }

    public static void Main()
    {
        const int tileSize = 32;
        
        var input = File.ReadAllText("start.txt");

        var lines = input
            .Split('\r', '\n')
            .Where(l => l != string.Empty)
            .ToArray();
 
        var gridWidth = lines.First().Length;
        var gridHeight = lines.Length;
        var grid = new bool[gridWidth, gridHeight];
        var neighbourMask = new int[gridWidth, gridHeight];
        var frame = 0;

        for (var y = 0; y < grid.GetLength(0); ++y)
        {
            Console.WriteLine(lines[y]);
            
            if (lines[y].Length != gridWidth)
                throw new Exception("Grid must not be jagged");
            
            for (var x = 0; x < grid.GetLength(1); ++x)
                grid[y, x] = lines[y][x] is '#';
        }

        var stateSet = new Dictionary<BigInteger, int>();

        Raylib.InitWindow(tileSize * gridWidth, tileSize * gridHeight, "GoL");
        var update = true;
        
        while (!Raylib.WindowShouldClose())
        {
            if (update)
            {
                Update(grid, neighbourMask);

                var state = GetGridState(grid);

                if (stateSet.TryGetValue(state, out var value))
                {
                    Console.WriteLine($"Cycle detected (start frame: {value}, end frame: {frame})");
                    update = false;
                }
                else
                {
                    stateSet.Add(state, frame);
                }
            
                frame++;    
            }
            
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.WHITE);

            for (var y = 0; y < gridHeight; ++y)
            {
                for (var x = 0; x < gridWidth; ++x)
                {
                    Raylib.DrawRectangle(
                        tileSize * x, 
                        tileSize * y,
                        tileSize,
                        tileSize,
                        grid[y, x] ? Color.WHITE : Color.BLACK);                    
                }
            }
            
            Raylib.EndDrawing();
            
            if(update)
                Thread.Sleep(75);
        }
        Raylib.CloseWindow();
    }
}