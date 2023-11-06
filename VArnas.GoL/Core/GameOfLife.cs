using System.Data;

namespace VArnas.GoL.Core;

public class GameOfLife
{
    private readonly bool[,] _grid;
    private readonly int[,] _neighbours;
    
    private int Height => _grid.GetLength(0);
    private int Width => _grid.GetLength(1);
    
    public GameOfLife(bool[,] grid)
    {
        _grid = grid;
        _neighbours = new int[Height, Width];
    }

    private void Iterate(Action<int, int> func)
    {
        for (var y = 0; y < Height; ++y)
            for (var x = 0; x < Width; ++x)
                func(y, x);
    }
    
    public void Update()
    {
        Iterate((y, x) => _neighbours[y, x] = CountNeighbours(x, y)); 
        Iterate((y, x) => _grid[y, x] = _grid[y, x] switch
        {
            true when _neighbours[y, x] is < 2 or > 3 => false,
            false when _neighbours[y, x] is 3 => true,
            _ => _grid[y, x]
        });
    }
        
    private int CountNeighbours(int x, int y) => new[]
        {
            (x - 1, y - 1), (x, y - 1), (x + 1, y - 1),
            (x - 1, y), (x + 1, y),
            (x - 1, y + 1), (x, y + 1), (x + 1, y + 1),
        }
        .Count(p => _grid[(Height + p.Item2) % Height, (Width + p.Item1) % Width]);
}