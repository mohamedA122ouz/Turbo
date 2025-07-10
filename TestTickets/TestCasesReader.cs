using System;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace TestTickets.test;

public class TestCasesReader
{
    private static string testPath = @"e:\git repo\CustomCRM\test";
    public static IEnumerable<object[]> Files()
    {
        var dir = testPath;

        foreach (var file in Directory.EnumerateFiles(dir, "*.json"))
        {
            var json = File.ReadAllText(file);
            var data = JsonConvert.DeserializeObject<TestCase>(json);

            yield return new Object[] { new NamedTestCase
            {
                FileName = file,
                Data = data
            }};
        }
    }
}
