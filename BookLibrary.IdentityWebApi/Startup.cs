namespace BookLibrary.IdentityWebApi
{
    using BookLibrary.Core.Commands;
    using BookLibrary.Core.Events;
    using BookLibrary.Core.Queries;
    using BookLibrary.DataAccess.Contexts;
    using Core.Commands.Identity;
    using Core.Models;
    using Core.Queries.Identity;
    using DataAccess.Models;
    using Identity.Handlers;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

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
            services.AddControllers();

            this.ConfigureMediator(services);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["ConnectionString"]));

            services.AddIdentity<Librarian, IdentityRole<int>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<ICommandBus, CommandBus>();

            services.AddScoped<IQueryBus, QueryBus>();

            services.AddScoped<IEventBus, EventBus>();

            services.AddScoped<IRequestHandler<GetTokenQuery, TokenResponse>, GetTokenHandler>();

            services.AddScoped<IRequestHandler<AddPersonCommand, Unit>, AddPersonHandler>();

            services.AddScoped<IRequestHandler<GetPersonQuery, Person>, GetPersonHandler>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "BookOnLoan Library - Identity",
                    Version = "v1"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookOnLoan Library - Identity"); });

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void ConfigureMediator(IServiceCollection services)
        {
            services.AddScoped<IMediator, Mediator>();

            services.AddTransient<ServiceFactory>(sp => t => sp.GetService(t));
        }
    }
}