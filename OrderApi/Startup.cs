using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OrderApi.Domain.Data;
using OrderApi.Infrastructure.Filters;
using Swashbuckle.AspNetCore.Swagger;

namespace OrderApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //_connectionString = $@"Server={Configuration["MYSQL_SERVER_NAME"]};
            //                       Database={Configuration["MYSQL_DATABASE"]};
            //                       Uid={Configuration["MYSQL_USER"]};
            //                       Pwd={Configuration["MYSQL_PASSWORD"]}";
            _connectionString = Configuration["ConnectionString"];
        }

        private readonly string _connectionString;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(Options =>
                {
                    Options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    Options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    
                });
            /* services.AddMvcCore(
                options => options.Filters.Add(typeof(HttpGlobalExceptionFilter))
            )
            .AddJsonFormatters(
                Options =>
                {
                    Options.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    Options.ContractResolver = new CamelCasePropertyNamesContractResolver();
                }
            )
            .AddApiExplorer();*/
            // WaitForDBInit(_connectionString);
             services.AddEntityFrameworkMySql()
            .AddDbContext<OrderContext>(options =>
                {
                    options.UseMySql(_connectionString,
                        mySqlOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                           
                        });
                },
                ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
            );
            services.Configure<OrderSettings>(Configuration);
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Ordering HTTP API",
                    Version = "v1",
                    Description = "The Ordering Service HTTP API",
                    TermsOfService = "Terms Of Service"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, OrderContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }
            context.Database.Migrate();
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "OrderApi V1");
                });
        }
        
        private void WaitForDBInit(string connectionString)
        {
            var connection = new MySqlConnection(connectionString);
            int retries = 1;
            while (retries < 7)
            {
                try
                {
                    Console.WriteLine($"Connecting to db. Trial: {retries} - {connectionString}");
                    connection.Open();
                    connection.Close();
                    break;
                }
                catch (MySqlException)
                {
                    Thread.Sleep(1000);
                    retries++;
                }
            }
        }
    }
}