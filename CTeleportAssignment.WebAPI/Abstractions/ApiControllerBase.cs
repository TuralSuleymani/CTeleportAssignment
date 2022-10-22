using CSharpFunctionalExtensions;
using CTeleportAssignment.WebAPI.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CTeleportAssignment.WebAPI.Abstractions
{
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IMediator Mediator { get; }

        protected ApiControllerBase(IMediator mediator)
        {
            Mediator = mediator;
        }
        
        protected async Task<IActionResult> Send<TResponse>(IRequest<Maybe<TResponse>> request) where TResponse : class
         {
             var response  = await Mediator.Send(request);
             return Ok(new ApiResponse<TResponse>(response.Value));
         }
        
    }
}
