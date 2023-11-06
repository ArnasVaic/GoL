using System.Data;
using System.Numerics;
using VArnas.GoL.Extensions;

namespace VArnas.GoL.Core;

public class GameOfLife
{
    private readonly bool[,] _grid;
    private readonly int[,] _neighbours;
    
    public int Height => _grid.GetLength(0);
    public int Width => _grid.GetLength(1);
    
    public GameOfLife(bool[,] grid)
    {
        _grid = grid;
        _neighbours = new int[Height, Width];
    }
    
    public void Update()
    {
        _grid.Iterate((x, y) => _neighbours[y, x] = CountNeighbours(x, y)); 
        _grid.Iterate((x, y) => _grid[y, x] = _grid[y, x] switch
        {
            true when _neighbours[y, x] is < 2 or > 3 => false,
            false when _neighbours[y, x] is 3 => true,
            _ => _grid[y, x]
        });
        Thread.Sleep(75);
    }

    public bool Alive(int x, int y) => _grid[y, x];
    
    public BigInteger GetState()
    {
        var state = new BigInteger(0);
        
        _grid.Iterate((x, y) =>
        {
            if (!_grid[y, x]) return;
            var index = x + y * Width;
            state += BigInteger.Pow(2, index);
        });
        
        return state;
    }
        
    private int CountNeighbours(int x, int y) => new[]
        {
            (x - 1, y - 1), (x, y - 1), (x + 1, y - 1),
            (x - 1, y), (x + 1, y),
            (x - 1, y + 1), (x, y + 1), (x + 1, y + 1),
        }
        .Count(p => _grid[(Height + p.Item2) % Height, (Width + p.Item1) % Width]);
}