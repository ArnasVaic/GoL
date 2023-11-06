namespace VArnas.GoL.Extensions;

public static class MultiDimensionalArrayExtensions
{
    public static void Iterate<T>(this T[,] array, Action<int, int> func)
    {
        for (var y = 0; y < array.GetLength(0); ++y)
            for (var x = 0; x < array.GetLength(1); ++x)
                func(x, y);
    }
}