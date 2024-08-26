class Split
{
    public static List<List<T>> ListToChunk<T>(List<T> source, int chunkSize)
    {
        List<List<T>> result = new List<List<T>>();
        for (int i = 0; i < source.Count; i += chunkSize)
        {
            List<T> chunk = source.GetRange(i, Math.Min(chunkSize, source.Count - i));
            result.Add(chunk);
        }
        return result;
    }
}