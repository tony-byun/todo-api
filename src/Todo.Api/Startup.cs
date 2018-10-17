using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Todo.api.Data;

namespace Todo.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation(configuration => 
                    configuration.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddMediatR();
            services.AddAutoMapper();
            services.AddDbContext<TodoContext>((provider, optionsBuilder) => 
                optionsBuilder.UseInMemoryDatabase("InMemory"));
            
            // swagger
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "10x10 Api",
                    Version = "v1",
                    Description = "",
                    TermsOfService = ""
                });
            });
            
            // autofac
            var builder = new ContainerBuilder();

            builder.Populate(services);

            // https://autofaccn.readthedocs.io/en/latest/register/registration.html
            // builder.RegisterType<SomeClass>().As<ISomeClass>;
                        
            var container = builder.Build();
            return container.Resolve<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
            }
            
            // swagger
            app.UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api V1");
            });
            

            app.UseMvc();
        }
    }
}