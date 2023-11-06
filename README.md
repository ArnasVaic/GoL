# Game Of Life

This is a simulation of Game of Life written in C#.

## Features

- Program detects if a simulation converged or got trapped in a loop (reports it via `stdout`) 
- Load configuration from file and run it (expected file `start.txt` in the same directory as executable)
- Pause/Run simulation (Space key)
- Toggle cells (Left/Right click on cell you want to toggle)

## Running

To run from source, `.NET` sdk should be installed (can be downloaded here: https://dotnet.microsoft.com/en-us/download/visual-studio-sdks)

After installing, the project can be launched by going to the `VArnas.Gol` directory (one level deeper than solution dir) and running:

```shell
dotnet run
```