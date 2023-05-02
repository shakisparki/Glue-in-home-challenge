using global::AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TT.Deliveries.Auth.Models;
using TT.Deliveries.Core.Enums;
using TT.Deliveries.Core.Extensions;
using TT.Deliveries.Data.Dto;
using TT.Deliveries.Services.Contracts;
using TT.Deliveries.Web.Api.Models;

namespace TT.Deliveries.Web.Api.Controllers
{
    [Route("deliveries/state")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class StateController : BaseController
    {
        private readonly IStateService _stateService;
        private readonly ILogger<StateController> _logger;

        public StateController(IStateService stateService, ILogger<StateController> logger)
        {
            _stateService = stateService;
            _logger = logger;
        }

        /// <summary>
        /// Approves a newly Created Delivery
        /// </summary>
        /// <returns></returns>
        /// <response code="200"> Successfully approved the delivery </response>
        /// <response code="400"> If the request is incorrect e.g. id is wrong, bad state transition, or delivery has expired </response>
        /// <response code="401"> If the user is not authenticated </response>
        /// <response code="403"> If the user lacks permission to approve a delivery </response>
        /// <response code="500"> If something has gone wrong while approving the delivery </response>
        /// <remarks>
        /// Sample request:
        ///     PUT /deliveries/state/7432D362-15B3-4CC6-BAB6-F417A5CD5265/Approve
        /// </remarks>
        [Authorize(Roles =
            Claims.Customer + "," +
            Claims.Admin + ",")]
        [HttpPut("{Id}/[Action]")]
        public async Task<IActionResult> Approve(Guid Id)
        {
            var response = await _stateService.UpdateAsync(Id, DeliveryState.Approved);
            return HandleResponse(response.Error);
        }


        /// <summary>
        /// Completes an Approved Delivery
        /// </summary>
        /// <returns></returns>
        /// <response code="200"> Successfully completed the delivery </response>
        /// <response code="400"> If the request is incorrect e.g. id is wrong, bad state transition, or delivery has expired </response>
        /// <response code="401"> If the user is not authenticated </response>
        /// <response code="403"> If the user lacks permission to complete a delivery </response>
        /// <response code="500"> If something has gone wrong while completing the delivery </response>
        /// <remarks>
        /// Sample request:
        ///     PUT /deliveries/state/7432D362-15B3-4CC6-BAB6-F417A5CD5265/Complete
        /// </remarks>
        [Authorize(Roles =
            Claims.Partner + "," +
            Claims.Admin + ",")]
        [HttpPut("{Id}/[Action]")]
        public async Task<IActionResult> Complete(Guid Id)
        {
            var response = await _stateService.UpdateAsync(Id, DeliveryState.Completed);
            return HandleResponse(response.Error);
        }

        /// <summary>
        /// Cancels a pending Delivery ((in state created or approved)
        /// </summary>
        /// <returns></returns>
        /// <response code="200"> Successfully cancelled the delivery </response>
        /// <response code="400"> If the request is incorrect e.g. id is wrong, bad state transition, or delivery has expired </response>
        /// <response code="401"> If the user is not authenticated </response>
        /// <response code="403"> If the user lacks permission to cancel a delivery </response>
        /// <response code="500"> If something has gone wrong while cancelling the delivery </response>
        /// <remarks>
        /// Sample request:
        ///     PUT /deliveries/state/7432D362-15B3-4CC6-BAB6-F417A5CD5265/Cancel
        /// </remarks>
        [Authorize(Roles =
            Claims.Partner + "," +
            Claims.Customer + "," +
            Claims.Admin + ",")]
        [HttpPut("{Id}/[Action]")]
        public async Task<IActionResult> Cancel(Guid Id)
        {
            var response = await _stateService.UpdateAsync(Id, DeliveryState.Cancelled);
            return HandleResponse(response.Error);
        }
    }
}
