using System.Numerics;
using Raylib_cs;
using VArnas.GoL.Core;

namespace VArnas.GoL;

public class Program
{
    private const int TileSize = 32;
    private const int FrameTimeMs = 75;
    
    private int _frame; // Current frame
    private bool _update; // Pause/unpause simulation
    private GameOfLife Game { get; }
    private readonly Dictionary<BigInteger, int> _stateCache;
    private bool _cycleFound; // Look for cycles
    
    private Program()
    {
        _frame = 0;
        var initial = Helpers.LoadInitialState("start.txt");
        Game = new GameOfLife(initial);
        _stateCache = new Dictionary<BigInteger, int>();
        _update = false;
        
        Raylib.SetTraceLogLevel(TraceLogLevel.LOG_ERROR);
        
        Raylib.InitWindow(
            TileSize * Game.Width, 
            TileSize * Game.Height, 
            "GoL");
    }

    private void Update()
    {
        var pos = Raylib.GetMousePosition();
        var x = (int)pos.X / TileSize;
        var y = (int)pos.Y / TileSize;

        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            Game.Set(x, y, true);
        
        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_RIGHT))
            Game.Set(x, y, false);
        
        if(Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            _update = !_update;

        if (!_update) return;
        
        Game.Update();
        var state = Game.GetState();
        
        if (_stateCache.TryGetValue(state, out var savedFrame) && !_cycleFound)
        {
            var period = _frame - savedFrame;
            Console.WriteLine(period is 1
                ? "State converged."
                : $"Cycle detected (start frame: {savedFrame}, end frame: {_frame}).");
            _cycleFound = true;
        }
        else if(!_cycleFound) _stateCache.Add(state, _frame);
        _frame++;
        
        Thread.Sleep(FrameTimeMs);
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