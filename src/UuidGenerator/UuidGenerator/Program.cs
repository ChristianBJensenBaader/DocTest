namespace UuidGenerator
{
  internal class Program
  {
    private static  async IAsyncEnumerable<Guid> GetSequentialGuidAsync(int max)
    {
      for (int i = 0; i < max; i++)
      {
        yield return UuidGenerator.NewUuid7();

        await Task.Delay(100);
      }
    }
    
    static async Task Main(string[] args)
    {
      //var uuid7 = UuidGenerator.NewUuid7();
      //var uuid4 = UuidGenerator.NewUuid4();

      List<KeyValuePair<DateTime, Guid>> guidMap = new();

      await foreach (var uuid7 in GetSequentialGuidAsync(10))
      {
        guidMap.Add(new KeyValuePair<DateTime, Guid>(DateTime.UtcNow, uuid7));
      }

      Console.WriteLine("As generated");
      Console.WriteLine("-----------------------------------");
      foreach (var (when, guid) in guidMap.OrderBy(x => x.Key))
      {
        Console.WriteLine($"Time: {when.Ticks}, guid: {guid}");
      }
      Console.WriteLine();
      Console.WriteLine("As sorted");
      Console.WriteLine("-----------------------------------");
      foreach (var (when, guid) in guidMap.OrderBy(x => x.Value))
      {
        Console.WriteLine($"Time: {when.Ticks}, guid: {guid}");
      }
      
      //Console.WriteLine($"UUID version 7: {uuid7}");
      //Console.WriteLine($"UUID version 4: {uuid4}");
    }
  }
}
