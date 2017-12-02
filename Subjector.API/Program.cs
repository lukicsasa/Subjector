using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Subjector.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel(options =>
                {
                    var httpsConnectionAdapterOptions = new HttpsConnectionAdapterOptions()
                    {
                        ClientCertificateMode = ClientCertificateMode.AllowCertificate,
                        SslProtocols = System.Security.Authentication.SslProtocols.Tls,
                        ServerCertificate = new X509Certificate2("localhost.pfx", "exitia")
                    };

                    options.Listen(IPAddress.Loopback, 5000);
                    options.Listen(IPAddress.Loopback, 5001, listenOptions =>
                    {
                        listenOptions.UseHttps(httpsConnectionAdapterOptions);
                    });
                })
                .UseUrls("https://*:5001")
                .Build();
    }
}
