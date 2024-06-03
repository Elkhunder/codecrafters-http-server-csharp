﻿using System.Net;
using codecrafters_http_server.Handlers;
using static System.Net.Mime.MediaTypeNames;

namespace codecrafters_http_server.Helpers;

public abstract class HttpResponseBuilder()
{
    public static HttpResponse Build(ResponseStatusLine responseStatusLine, ResponseEntity responseEntity)
    {
        return new HttpResponse(responseStatusLine, responseEntity);
    }

    public static ResponseStatusLine BuildStatusLine(Route route, HttpMethod method, HttpRequest request)
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
                var file = request.RequestFile;
                if (method == HttpMethod.Get && file.FileValidated)
                {
                    return new ResponseStatusLine(HttpStatusCode.OK);
                }
                else if (method == HttpMethod.Get && !file.FileValidated)
                {
                    return new ResponseStatusLine(HttpStatusCode.NotFound);
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
                var userAgentHeader = request.Headers.First(header => header.Name == "User-Agent");
                return new ResponseEntity([
                    new ResponseHeader<string>(HttpResponseHeader.ContentType, Text.Plain),
                    new ResponseHeader<int>(HttpResponseHeader.ContentLength, userAgentHeader.Value.Length)], userAgentHeader.Value);
            case Routes.Files:
                var file = request.RequestFile;
                if (method == HttpMethod.Get && file.FileValidated && !string.IsNullOrEmpty(file.Contents))
                {
                    Console.WriteLine($"###{nameof(HttpResponseBuilder)}\r\n" +
                                      $"#####{nameof(RequestFile)}-{nameof(RequestFile.Contents)}: {request.RequestFile.Contents}" +
                                      $"\r\n" +
                                      $"#####{nameof(RequestFile.Contents)}-{nameof(RequestFile.Contents.Length)}: {request.RequestFile.Contents.Length}" +
                                      $"\r\n");
                    return new ResponseEntity([
                        new ResponseHeader<string>(
                            HttpResponseHeader.Connection, "close"),
                        new ResponseHeader<string>(
                            HttpResponseHeader.ContentType, Application.Octet),
                        new ResponseHeader<int>(
                            HttpResponseHeader.ContentLength, request.RequestFile.Contents.Length),
                        ], request.RequestFile.Contents);
                }

                if (method == HttpMethod.Get && !file.FileValidated)
                {
                    return new ResponseEntity([
                        new ResponseHeader<string>(HttpResponseHeader.Connection, "close"),
                        new ResponseHeader<int>(HttpResponseHeader.ContentLength, 0)
                    ], request.RequestFile.Contents);
                }
                else if (method == HttpMethod.Post && file.FileValidated)
                {
                    return new ResponseEntity([new ResponseHeader<string>(HttpResponseHeader.ContentType, Application.Octet)], request.Body);
                }
                else
                {
                    Console.WriteLine(new NotSupportedException
                        (
                            $"{nameof(route.Path)}: {route.Path}, " +
                            $"{nameof(method)}: {method}, " +
                            $"{nameof(request.RequestFile)}-{nameof(request.RequestFile.Contents)}-{nameof(request.RequestFile.Contents.Length)}: {request.RequestFile.Contents.Length}"
                        ));
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