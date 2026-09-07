using AutoMapper;
using EstadoDeCuenta.Domain.Entities;
using EstadoDeCuenta.DTOs.Cards;
using EstadoDeCuenta.DTOs.Clients;
using EstadoDeCuenta.DTOs.Movements;
using EstadoDeCuenta.Services.CQRS.Commands.CreateMovement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Client, ClientResponseDto>();
            CreateMap<Card, CardResponseDto>();
            CreateMap<Movement, MovementResponseDto>();
            CreateMap<CreateMovementCommand, Movement>();
            CreateMap<Movement, MovementResponseDto>();
        }
    }
}
