using BankMore.Transferencia.Application.Services;
using BankMore.Transferencia.Domain.Interfaces;
using BankMore.Transferencia.Infrastructure.Data;
using BankMore.Transferencia.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using KafkaFlow;
using KafkaFlow.Serializer;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// ---------------- DbContext ----------------
// Troque UseInMemoryDatabase por UseSqlite ou UseOracle para produção
builder.Services.AddDbContext<TransferenciaContext>(options =>
//    options.UseInMemoryDatabase("TransferenciaDb"));
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));



// ---------------- Repositórios ----------------
builder.Services.AddScoped<ITransferenciaRepository, TransferenciaRepository>();
builder.Services.AddScoped<IIdempotenciaRepository, IdempotenciaRepository>();

// ---------------- Serviços ----------------
builder.Services.AddHttpClient(); // Necessário para ContaCorrenteApiService
builder.Services.AddScoped<IContaCorrenteApiService, ContaCorrenteApiService>();

// ---------------- Mediator ----------------
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(EfetuarTransferenciaHandler).Assembly);
});

// ---------------- Kafka ----------------
builder.Services.AddKafkaFlow(
    kafka => kafka
        .UseLogProvider(new KafkaFlow.Common.LogProviders.ConsoleLogProvider())
        .AddCluster(
            cluster => cluster
                .WithBrokers(new[] { "localhost:9092" })
                .AddProducer(
                    producer => producer
                        .DefaultTopic("transferencias-realizadas")
                        .WithSerializer<JsonMessageSerializer>() // Serializa objeto para JSON
                )
        )
);

// ---------------- Controllers e Swagger ----------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------------- JWT (Autenticação) ----------------
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//       .AddJwtBearer(options => { ... });

// ---------------- Build app ----------------
var app = builder.Build();

// ---------------- Middl
