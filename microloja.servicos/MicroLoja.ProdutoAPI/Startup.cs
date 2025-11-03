using System.Reflection;
using FluentValidation;
using MicroLoja.ProdutoAPI.Aplicacao.Interfaces;
using MicroLoja.ProdutoAPI.Aplicacao.Servicos;
using MicroLoja.ProdutoAPI.Aplicacao.Validacoes;
using MicroLoja.ProdutoAPI.Dominio.Interfaces;
using MicroLoja.ProdutoAPI.Dominio.Interfaces.Base;
using MicroLoja.ProdutoAPI.Dominio.Modelos;
using MicroLoja.ProdutoAPI.Dominio.Notificacoes;
using MicroLoja.ProdutoAPI.Infraestrutura.Contexto;
using MicroLoja.ProdutoAPI.Infraestrutura.Repositorios;
using MicroLoja.ProdutoAPI.Infraestrutura.Repositorios.Base;
using MicroLoja.ProdutoAPI.Infraestrutura.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace MicroLoja.ProdutoAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ProdutoDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            #region Injeção de Dependências
            // Registro do Notificador (deve ser antes dos repositórios)
            services.AddScoped<INotificador, Notificador>();

            // Registro do Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Registro dos Repositórios
            services.AddScoped(typeof(IRepositorioBase<>), typeof(RepositorioBase<>));
            services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();

            // Registro dos Serviços de Aplicação
            services.AddScoped<IProdutoServico, ProdutoServico>();
            services.AddScoped<ICategoriaServico, CategoriaServico>();

            // Registro do FluentValidation
            services.AddScoped<IValidator<Produto>, ProdutoValidacao>();
            services.AddScoped<IValidator<Categoria>, CategoriaValidacao>();
            #endregion

            services.AddControllers();

            #region Configuração do Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Loja Online - Produto API",
                    Version = "v1",
                    Description = "API para gerenciamento de produtos da Loja Online",
                    Contact = new OpenApiContact() { Name = "Emmilly Lins", Email = "emycmlins@gmail.com" }
                });

                // Configuração para incluir comentários XML
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                // Configuração para melhorar a documentação
                c.DescribeAllParametersInCamelCase();

                // Configuração para autenticação JWT no Swagger (se necessário)
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header usando o esquema Bearer. Exemplo: \"Authorization: Bearer {token}\""
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
            #endregion

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            services.AddAutoMapper(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = string.Empty;
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Loja Online - Produto API v1");
                    c.DocumentTitle = "Loja Online - Produto API";
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
                    c.DefaultModelsExpandDepth(-1);
                    c.DisplayRequestDuration();
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("AllowAllOrigins");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}