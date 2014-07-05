using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using Dapper;
using DapperExtensions;
using Nancy;

namespace BuildNumberService
{
    public class HomeModule : NancyModule
    {
        public HomeModule(Operations ops)
        {
            Get["/"] = _ => View["Index", ops.List()];

            Get["/db"] = _ =>
            {                
                ops.CreateDatabase();

                return "Database created";
            };

            Get["/next/{key*}"] = _ => ops.NextNumber(_.key).ToString();

            Get["/confirm/{key*}"] = _ =>
            {
                ops.Confirm(_.key, Request.Query.version);

                return "OK";
            };

            Get["/autoconfirm/{key*}"] = _ =>
            {
                var number = ops.NextNumber(_.key);
                
                ops.Confirm(_.key, number);

                return number.ToString();
            };
        }
    }
}