using EstadoDeCuenta.Domain.Interfaces;
using EstadoDeCuenta.DTOs.Cards;
using EstadoDeCuenta.DTOs.Clients;
using EstadoDeCuenta.DTOs.Movements;
using EstadoDeCuenta.Infrastructure.Data;
using EstadoDeCuenta.Infrastructure.Repositories;
using EstadoDeCuenta.Infrastructure.UnitOfWork;
using EstadoDeCuenta.Services.CQRS;
using EstadoDeCuenta.Services.CQRS.Commands.CreateCard;
using EstadoDeCuenta.Services.CQRS.Commands.CreateClient;
using EstadoDeCuenta.Services.CQRS.Commands.CreateMovement;
using EstadoDeCuenta.Services.CQRS.Queries.GetCards;
using EstadoDeCuenta.Services.CQRS.Queries.GetClients;
using EstadoDeCuenta.Services.CQRS.Queries.GetMovements;
using EstadoDeCuenta.Services.Mapping;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IMovementRepository, MovementRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICommandHandler<CreateMovementCommand, MovementResponseDto>, CreateMovementCommandHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateMovementCommandValidator>();
builder.Services.AddScoped<ICommandHandler<CreateClientCommand, ClientResponseDto>, CreateClientCommandHandler>();
builder.Services.AddScoped<ICommandHandler<CreateCardCommand, CardResponseDto>, CreateCardCommandHandler>();
builder.Services.AddScoped<IQueryHandler<GetClientsQuery, IEnumerable<ClientResponseDto>>, GetClientsQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetCardsQuery, IEnumerable<CardResponseDto>>, GetCardsQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetMovementsQuery, IEnumerable<MovementResponseDto>>, GetMovementsQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetClientByIdQuery, ClientResponseDto>,GetClientByIdQueryHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
