namespace CustomExtensions
{
    public static class ArrayExtensions
    {
        public static int IndexOf<T>(this T[] array, T obj) where T : class
        {
            for (var i = 0; i < array.Length; i++)
            {
                var elt = array[i];
                if (elt == obj) return i;
            }
            return -1;
        }
    }
}
