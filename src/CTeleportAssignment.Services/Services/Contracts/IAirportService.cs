﻿using CSharpFunctionalExtensions;
using CTeleportAssigment.Domain;
using CTeleportAssignment.Services.Models;

namespace CTeleportAssignment.Services.Services.Contracts
{
    public interface IAirportService
    {
        Task<Result<AirportPair, DomainError>> CalculateDistanceAsync(Iata firstIata, Iata secondIata, CancellationToken cancellationToken);
    }
}
