using codecrafters_http_server.Helpers;

namespace codecrafters_http_server.Extensions;

public static class ListExtensions
{
    public static bool TryGetValue<T> (this List<T> list, string headerName, out T? headerValue)
        where T : RequestHeader
    {
        var header = list.FirstOrDefault(header => header.Name.ToString() == headerName);
        if (header is null)
        {
            headerValue = default;
            return false;
        }

        headerValue = header;
        return true;
    }
}