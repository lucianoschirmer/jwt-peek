if (args.Length == 0)
{
    Console.Error.WriteLine("uso: jwt-peek <token>");
    return 1;
}

var parts = args[0].Split('.');
if (parts.Length < 2)
{
    Console.Error.WriteLine("isso não parece um JWT (esperava pelo menos header.payload)");
    return 1;
}

Console.WriteLine("HEADER");
Console.WriteLine(PrettyPrint(parts[0]));
Console.WriteLine();
Console.WriteLine("PAYLOAD");
Console.WriteLine(PrettyPrint(parts[1]));

return 0;

static string PrettyPrint(string base64UrlSegment)
{
    var json = System.Text.Encoding.UTF8.GetString(Base64UrlDecode(base64UrlSegment));
    using var doc = System.Text.Json.JsonDocument.Parse(json);
    return System.Text.Json.JsonSerializer.Serialize(doc, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
}

static byte[] Base64UrlDecode(string input)
{
    var padded = input.Replace('-', '+').Replace('_', '/');
    switch (padded.Length % 4)
    {
        case 2: padded += "=="; break;
        case 3: padded += "="; break;
    }
    return Convert.FromBase64String(padded);
}
