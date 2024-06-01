using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace codecrafters_http_server.Helpers;

public abstract class HttpResponseBuilder()
{
    public static HttpResponse Build(ResponseStatusLine responseStatusLine, ResponseEntity responseEntity)
    {
        return new HttpResponse(responseStatusLine, responseEntity);
    }

    public static ResponseStatusLine BuildStatusLine(Route route, HttpMethod method)
    {
        switch (route.Path)
        {
            case Routes.Default:
                return new ResponseStatusLine(HttpStatusCode.OK);
            case Routes.Echo:
                return new ResponseStatusLine(HttpStatusCode.OK);
            case Routes.UserAgent:
                return new ResponseStatusLine(HttpStatusCode.OK);
            case Routes.Files:
                if (method == HttpMethod.Get)
                {
                    return new ResponseStatusLine(HttpStatusCode.OK);
                }
                else if (method == HttpMethod.Post)
                {
                    return new ResponseStatusLine(HttpStatusCode.Created);
                }
                else
                {
                    Console.WriteLine(new NotSupportedException(nameof(method)));
                    return new ResponseStatusLine(HttpStatusCode.InternalServerError);
                }
            case Routes.NotFound:
                return new ResponseStatusLine(HttpStatusCode.NotFound);
            default:
                Console.WriteLine(new NotSupportedException(nameof(route.Path)));
                return new ResponseStatusLine(HttpStatusCode.InternalServerError);
        }   
    }

    public static ResponseEntity BuildResponseEntity(Route route, HttpRequest request, HttpMethod method)
    {
        switch (route.Path)
        {
            case Routes.Default:
                return new ResponseEntity([new ResponseHeader<int>(HttpResponseHeader.ContentLength, request.Body.Length)], request.Body);
            case Routes.Echo:
                return new ResponseEntity([new ResponseHeader<string>(HttpResponseHeader.ContentType, Text.Plain)], request.Body);
            case Routes.UserAgent:
                return new ResponseEntity([new ResponseHeader<string>(HttpResponseHeader.ContentType, Text.Plain)], request.Body);
            case Routes.Files:
                if (method == HttpMethod.Get && request.Body is not null)
                {
                    return new ResponseEntity([
                        new ResponseHeader<string>(
                            HttpResponseHeader.ContentType, Application.Octet),
                        new ResponseHeader<int>(
                            HttpResponseHeader.ContentLength, request.Body.Length),
                        ], request.Body);
                }
                else if (method == HttpMethod.Post)
                {
                    return new ResponseEntity([new ResponseHeader<string>(HttpResponseHeader.ContentType, Application.Octet)], request.Body);
                }
                else
                {
                    Console.WriteLine(new NotSupportedException($"{nameof(route.Path)}, {nameof(method)}"));
                    return ResponseEntity.Empty;
                }
            case Routes.NotFound:
                return ResponseEntity.Empty;
            default:
                Console.WriteLine(new NotSupportedException(nameof(route.Path)));
                return ResponseEntity.Empty;
        }
    }
}