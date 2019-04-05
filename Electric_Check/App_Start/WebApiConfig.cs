using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Electric_Check
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "Electric_Check/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // 序列化出来的JSON，包含了为NULL的字段，导致swagger-ui-min-js出现异常
            // 因为我项目使用的newtonsoft.json这个库的配置导致，应该忽略为NULL的字段
            // 解决办法
            var jsonFormatter = new JsonMediaTypeFormatter();
            var settings = jsonFormatter.SerializerSettings;

            settings.NullValueHandling = NullValueHandling.Ignore;
            //config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter));
        }
    }
}
