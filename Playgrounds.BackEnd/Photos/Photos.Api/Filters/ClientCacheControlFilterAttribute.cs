using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
//using System.Web.Http;

namespace Photo.Api.Filters
{
    public enum ClientCacheControl
    {
        Public,
        Private,
        NoCache
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ClientCacheControlFilterAttribute : ActionFilterAttribute
    {
        public ClientCacheControl CacheType;
        public double CacheSeconds;

        public ClientCacheControlFilterAttribute(ClientCacheControl cacheType, double cacheSeconds)
        {
            CacheType = cacheType;
            CacheSeconds = cacheSeconds;
            if (CacheType == ClientCacheControl.NoCache)
            {
                CacheSeconds = -1;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            if (CacheType == ClientCacheControl.NoCache)
            {
                NoCacheHeaders(context);
            }
            else
            {
                AddCacheHeaders(context);
            }
        }

        private void AddCacheHeaders(ActionExecutedContext context)
        {
            if (context.HttpContext.Response.StatusCode == 200)
            {
                var cacheControlHeaderValue = new CacheControlHeaderValue()
                {
                    NoCache = false,
                    Public = (CacheType == ClientCacheControl.Public),
                    Private = (CacheType == ClientCacheControl.Private),
                    MaxAge = TimeSpan.FromSeconds(CacheSeconds)
                };

                context.HttpContext.Response.Headers.Add("Cache-Control", cacheControlHeaderValue.ToString());
                StringValues dateValue;
                context.HttpContext.Response.Headers.TryGetValue("Date", out dateValue);
                if (string.IsNullOrEmpty(dateValue))
                {
                    context.HttpContext.Response.Headers.Add("Date", DateTimeOffset.UtcNow.ToString());
                    context.HttpContext.Response.Headers.TryGetValue("Date", out dateValue);
                }

                context.HttpContext.Response.Headers.Add("Expires", DateTimeOffset.UtcNow.AddSeconds(CacheSeconds).ToString());
            }
        }

        private static void NoCacheHeaders(ActionExecutedContext context)
        {
            var headerCache = new CacheControlHeaderValue()
            {
                NoCache = true
            };

            context.HttpContext.Response.Headers.Add("Cache-Control", headerCache.ToString());

            StringValues dateValue;
            context.HttpContext.Response.Headers.TryGetValue("Date", out dateValue);

            if (string.IsNullOrEmpty(dateValue))
            {
                context.HttpContext.Response.Headers.Add("Date", DateTimeOffset.UtcNow.ToString());
            }

            context.HttpContext.Response.Headers.Add("Expires", DateTimeOffset.UtcNow.ToString());
        }
    }
}