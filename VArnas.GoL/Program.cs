using System.Numerics;
using Raylib_cs;
using VArnas.GoL.Core;

namespace VArnas.GoL;

public class Program
{
    private const int TileSize = 32;

    private int _frame;
    private bool _update;
    private GameOfLife Game { get; }
    private readonly Dictionary<BigInteger, int> _stateCache;
    
    private Program()
    {
        _frame = 0;
        var initial = Helpers.LoadInitialState("start.txt");
        Game = new GameOfLife(initial);
        _stateCache = new Dictionary<BigInteger, int>();
        _update = true;
        
        Raylib.InitWindow(
            TileSize * Game.Width, 
            TileSize * Game.Height, 
            "GoL");
    }

    private void Update()
    {
        if (!_update) return;
        
        Game.Update();

        var state = Game.GetState();
        if (_stateCache.TryGetValue(state, out var value))
        {
            Console.WriteLine($"Cycle detected (start frame: {value}, end frame: {_frame})");
            _update = false;
        }
        else _stateCache.Add(state, _frame);
        _frame++;
    }

    private void Draw()
    {
        for (var y = 0; y < Game.Height; ++y)
        {
            for (var x = 0; x < Game.Width; ++x)
            {
                Raylib.DrawRectangle(
                    TileSize * x, 
                    TileSize * y,
                    TileSize,
                    TileSize,
                    Game.Alive(x, y) ? Color.WHITE : Color.BLACK);                    
            }
        }
    }
    
    public static void Main()
    {
        var program = new Program();
        while (!Raylib.WindowShouldClose())
        {
            program.Update();
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.WHITE);
            program.Draw();
            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }
}