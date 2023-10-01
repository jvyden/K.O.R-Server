﻿using System.Net;
using Bunkum.Listener.Request;
using Bunkum.Core.Database;
using Bunkum.Core.Endpoints.Middlewares;
using Bunkum.Protocols.Http;
using K.O.R_Server.Endpoints;

namespace K.O.R_Server.Middlewares;

public class CrossOriginMiddleware : IMiddleware
{
    private static readonly List<string> AllowedMethods = new();

    static CrossOriginMiddleware()
    {
        foreach (HttpMethods method in Enum.GetValues<HttpMethods>())
        {
            if(method == HttpMethods.Options) continue;
            AllowedMethods.Add(method.ToString().ToUpperInvariant());
        }
    }
    
    public void HandleRequest(ListenerContext context, Lazy<IDatabaseContext> database, Action next)
    {
        // Allow any origin for API
        // Mozilla says this is okay:
        //   "You can also configure a site to allow any site to access it by using the * wildcard. You should only use this for public APIs."
        // https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS/Errors/CORSMissingAllowOrigin#what_went_wrong
        if (context.Uri.AbsolutePath.StartsWith(ApiEndpointAttribute.BaseRoute))
        {
            context.ResponseHeaders.Add("Access-Control-Allow-Origin", "*");
            
            if (context.Method == HttpProtocolMethods.Options)
            {
                context.ResponseHeaders.Add("Access-Control-Allow-Headers", "Authorization, Content-Type");
                context.ResponseHeaders.Add("Access-Control-Allow-Methods", string.Join(", ", AllowedMethods));
                
                context.ResponseCode = HttpStatusCode.OK;
                return;
            }
        }
        
        next();
    }
}