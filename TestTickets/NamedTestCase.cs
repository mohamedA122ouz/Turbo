using System;

namespace TestTickets;

public class NamedTestCase
{
    public string FileName { get; set; }
    public TestCase Data { get; set; }
    public override string ToString() => $"Case_{Path.GetFileNameWithoutExtension(FileName)}";
}
