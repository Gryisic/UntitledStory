namespace Core.Extensions
{
    public static class IntExtensions
    {
        public static int Cycled(this int value, int excludedUpperBorder) => value + 1 >= excludedUpperBorder ? 0 : value + 1;
        
        public static int ReverseCycled(this int value, int excludedUpperBorder) => value - 1 < 0 ? excludedUpperBorder - 1 : value - 1;
    }
}