namespace BookLibrary.LibraryWebApi
{
    using System.Text;
    using Core.Commands;
    using Core.Commands.Library;
    using Core.Events;
    using Core.Events.Library;
    using Core.Models;
    using Core.Queries;
    using Core.Queries.Library;
    using DataAccess.Repositories;
    using DataAccess.Repositories.API;
    using Library.Handlers;
    using MediatR;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using MongoDB.Driver;
    using Swashbuckle.AspNetCore.Swagger;

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

            services.AddAuthentication(item =>
                {
                    item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    item.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidAudience = Configuration["JwtAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSecurityKey"]))
                    };

                });

            this.ConfigureMediator(services);

            services.AddSingleton(new MongoClient(Configuration["ConnectionString"]));

            services.AddScoped(typeof(IEventsRepository<>), typeof(EventsRepository<>));

            services.AddScoped<ICommandBus, CommandBus>();

            services.AddScoped<IQueryBus, QueryBus>();

            services.AddScoped<IEventBus, EventBus>();

            services.AddScoped<IRequestHandler<AddBookCommand, Unit>, AddBookHandler>();

            services.AddScoped<IRequestHandler<RemoveBookCommand, Unit>, RemoveBookHandler>();

            services.AddScoped<IRequestHandler<GetBookDetailsQuery, BookDetails>, GetBooksDetailsHandler>();

            services.AddScoped<INotificationHandler<BookCreatedEvent>, BookCreatedHandler>();

            services.AddScoped<INotificationHandler<BookRemovedEvent>, BookRemovedHandler>();

            services.AddScoped<INotificationHandler<BookLentEvent>, BookLentHandler>();

            services.AddScoped<IRequestHandler<LendBookCommand, Unit>, LendBookHandler>();

            services.AddScoped<IRequestHandler<GetPersonDetailsQuery, PersonBook>, GetPersonDetailsHandler>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Book Library",
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

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Books Library"); });

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void ConfigureMediator(IServiceCollection services)
        {
            services.AddScoped<IMediator, Mediator>();

            services.AddTransient<ServiceFactory>(sp => t => sp.GetService(t));
        }
    }
}