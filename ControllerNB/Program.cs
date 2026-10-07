using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.Json;

[AttributeUsage(AttributeTargets.Class)]
public class RouteAttribute : Attribute
{
    public string Template { get; }
    public RouteAttribute(string template) => Template = template;
}

public abstract class HttpMethodAttribute : Attribute
{
    public string Method { get; }
    protected HttpMethodAttribute(string method) => Method = method;
}

public class HttpGetAttribute : HttpMethodAttribute { public HttpGetAttribute() : base("GET") { } }
public class HttpPostAttribute : HttpMethodAttribute { public HttpPostAttribute() : base("POST") { } }
public class HttpDeleteAttribute : HttpMethodAttribute { public HttpDeleteAttribute() : base("DELETE") { } }

public class FromQueryAttribute : Attribute { }
public class FromBodyAttribute : Attribute { }

public class VideoDTO
{
    public string Name { get; set; } = "";
}

[Route("api/[controller]/[action]")]
public class WatchController
{
    [HttpGet]
    public string PlayVideo() => "Playing video";

    [HttpDelete]
    public string DeleteVideo([FromQuery] int id, [FromQuery] string name) => $"Deleted video {id} ({name})";

    [HttpPost]
    public string AddVideo([FromBody] VideoDTO video) => $"Added video {video.Name}";
}

class Program
{
    static string BuildPath(Type type, MethodInfo method) =>
        type.GetCustomAttribute<RouteAttribute>()!.Template
            .Replace("[controller]", type.Name.Replace("Controller", ""))
            .Replace("[action]", method.Name);

    static string Execute(string httpMethod, string url, string body = "")
    {
        if (url.Contains("://"))
            url = new Uri(url).PathAndQuery;

        string[] parts = url.Split('?');
        string path = parts[0].Trim('/');

        Dictionary<string, string> query = parts.Length > 1
            ? parts[1].Split('&').Select(p => p.Split('=')).ToDictionary(p => p[0], p => p[1], StringComparer.OrdinalIgnoreCase)
            : new Dictionary<string, string>();

        var candidates = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.Name.EndsWith("Controller"))
            .SelectMany(t => t.GetMethods(), (t, m) => new { Type = t, Method = m })
            .Where(x =>
                x.Method.GetCustomAttribute<HttpMethodAttribute>() != null &&
                BuildPath(x.Type, x.Method).Equals(path, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (candidates.Count == 0)
            return $"404 Not Found: /{path}";

        var endpoint = candidates.FirstOrDefault(x =>
            x.Method.GetCustomAttribute<HttpMethodAttribute>()!.Method == httpMethod);

        if (endpoint == null)
            return $"405 Method Not Allowed: {httpMethod} /{path}";

        object?[] args = endpoint.Method.GetParameters()
            .Select(p => p.GetCustomAttribute<FromBodyAttribute>() != null
                ? JsonSerializer.Deserialize(body, p.ParameterType, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                : Convert.ChangeType(query[p.Name!], p.ParameterType, CultureInfo.InvariantCulture))
            .ToArray();

        object controller = Activator.CreateInstance(endpoint.Type)!;
        return "200 OK: " + endpoint.Method.Invoke(controller, args);
    }

    static void Main()
    {
        Console.WriteLine(Execute("GET", "https://localhost/api/watch/PlayVideo?id=asd"));
        Console.WriteLine(Execute("POST", "https://localhost/api/watch/PlayVideo"));
        Console.WriteLine(Execute("POST", "https://localhost/api/watch/AddVideo", "{\"name\":\"test\"}"));
        Console.WriteLine(Execute("DELETE", "https://localhost/api/watch/DeleteVideo?id=5&name=test"));
    }
}
