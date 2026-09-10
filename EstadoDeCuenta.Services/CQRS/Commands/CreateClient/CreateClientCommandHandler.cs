using AutoMapper;
using EstadoDeCuenta.Domain.Entities;
using EstadoDeCuenta.Domain.Interfaces;
using EstadoDeCuenta.DTOs.Clients;
using EstadoDeCuenta.Services.CQRS;
using FluentValidation;

namespace EstadoDeCuenta.Services.CQRS.Commands.CreateClient;

public class CreateClientCommandHandler : ICommandHandler<CreateClientCommand, ClientResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateClientCommand> _validator;

    public CreateClientCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateClientCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<ClientResponseDto> HandleAsync(CreateClientCommand command)
    {
        var validationResult = await _validator.ValidateAsync(command);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var client = _mapper.Map<Client>(command);

        await _unitOfWork.Clients.AddAsync(client);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ClientResponseDto>(client);
    }
}

