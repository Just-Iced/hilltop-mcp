using Microsoft.Extensions.Logging;

namespace Mcp.Function;

public class HttpMCP
{
    private readonly ILogger<HttpMCP> _logger;

    public HttpMCP(ILogger<HttpMCP> logger)
    {
        _logger = logger;
    }
}