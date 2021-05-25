using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AspNetCoreRateLimit;
using Common.Interfaces;
using Common.Models;
using Link_Shortener.Config;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Services.Cache;
using Services.Database;

namespace Link_Shortener
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddControllers();

            services.AddMediatR(typeof(Program).GetTypeInfo().Assembly, typeof(Link).GetTypeInfo().Assembly);
            services.AddMvc().AddNewtonsoftJson();

            services.AddOptions();
            services.AddSingleton<ICacheService, SharedMemoryCacheService>();
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimit"));
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddHttpContextAccessor();

            if (GetConfig().IsUsingDynamo)
            {
                services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
                services.AddAWSService<IAmazonDynamoDB>();
                services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
                services.AddSingleton<IDatabaseService, DynamoDatabaseService>();
            }
            else
            {
                DbConfig dbSettings = GetMongoDbSettings();
                var client = new MongoClient(dbSettings.ConnectionString);
                IMongoDatabase db = client.GetDatabase(dbSettings.DatabaseName);
                var linksCollection = db.GetCollection<Link>(dbSettings.LinksCollectionName);
                services.AddSingleton(linksCollection);
                services.AddSingleton<IDatabaseService, MongoDatabaseService>();
            }

            if (GetConfig().IsUsingRedis)
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = GetRedisConfig().ConnectionString;
                    options.InstanceName = GetRedisConfig().InstanceName;
                });
            }
            else
            {
                services.AddMemoryCache();
                services.AddSingleton<ICacheService, SharedMemoryCacheService>();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseIpRateLimiting();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private DbConfig GetMongoDbSettings() => Configuration.GetSection(nameof(DbConfig)).Get<DbConfig>();

        private config GetConfig() => Configuration.GetSection(nameof(config)).Get<config>();

        private RedisConfig GetRedisConfig() => Configuration.GetSection(nameof(RedisConfig)).Get<RedisConfig>();
    }
}