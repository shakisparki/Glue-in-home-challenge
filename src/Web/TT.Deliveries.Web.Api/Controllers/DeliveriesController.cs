namespace TT.Deliveries.Web.Api.Controllers
{
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

    [Route("deliveries")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class DeliveriesController : BaseController
    {
        private readonly IDeliveryService _deliveryService;
        private readonly ILogger<DeliveriesController> _logger;
        private readonly IMapper _mapper;
        public DeliveriesController(
            IDeliveryService deliveryService,
            ILogger<DeliveriesController> logger,
            IMapper mapper)
        {
            _deliveryService = deliveryService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets all Deliveries
        /// </summary>
        /// <returns>Returns a list of deliveries</returns>
        /// <response code="200"> Successfully retrieved all deliveries</response>
        /// <response code="204"> If there are no delivery items</response>
        /// <response code="401"> If the user is not authenticated </response>
        /// <response code="403"> If the user lacks permission to get all deliveries </response>
        /// <response code="500"> If something has gone wrong while retrieving the list of deliveries </response>
        /// <remarks>
        /// Sample request:
        ///     GET /deliveries'
        /// </remarks>
        [Authorize(Roles =
            Claims.Partner+ "," +
            Claims.Admin + ",")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<BulkDelivery>>> Get()
        {
            var response = await _deliveryService.ReadAllAsync();
            var result = _mapper.Map<List<BulkDelivery>>(response.Value);
            return HandleResponse(response.Error, result);
        }

        /// <summary>
        /// Gets a Delivery by its Id
        /// </summary>
        /// <returns>Returns the delivery</returns>
        /// <response code="200"> Successfully retrieves the delivery </response>
        /// <response code="401"> If the user is not authenticated </response>
        /// <response code="403"> If the user lacks permission to get the delivery details </response>
        /// <response code="404"> If there are no delivery with that Id</response>
        /// <response code="500"> If something has gone wrong while retrieving the list of deliveries </response>
        /// <remarks>
        /// Sample request:
        ///     GET /deliveries/7432D362-15B3-4CC6-BAB6-F417A5CD5265
        /// </remarks>
        [Authorize(Roles =
            Claims.Partner + "," +
            Claims.Customer + "," +
            Claims.Admin + ",")]
        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Delivery>> GetById(Guid Id)
        {
            var response = await _deliveryService.ReadAsync(Id);
            var result = _mapper.Map<Delivery>(response.Value);
            return HandleResponse(response.Error, result);
        }

        /// <summary>
        /// Creates a new Delivery
        /// </summary>
        /// <returns>Returns the delivery created</returns>
        /// <response code="201"> Successfully created the delivery </response>
        /// <response code="400"> If the request is incorrect e.g. delivery is null or validation issues </response>
        /// <response code="401"> If the user is not authenticated </response>
        /// <response code="403"> If the user lacks permission to create a delivery </response>
        /// <response code="500"> If something has gone wrong while retrieving the list of deliveries </response>
        /// <remarks>
        /// Sample request:
        ///     POST /deliveries
        /// </remarks>
        [Authorize(Roles =
            Claims.Partner + "," +
            Claims.Admin + ",")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Delivery>> Post([FromBody] CreateDelivery newDelivery)
        {
            if (newDelivery is null)
            {
                _logger.LogError("Create delivery request is null");
                return BadRequest();
            }
            var dto = _mapper.Map<CreateDeliveryDto>(newDelivery);
            var response = await _deliveryService.CreateAsync(dto);

            var uri = new Uri($"{Request.Path.Value}/{response.Value.Id}", UriKind.Relative);
            var result = _mapper.Map<Delivery>(response.Value);
            return HandleResponse(response.Error, result, uri);
        }

        /// <summary>
        /// Updates a Delivery Specified with an Id
        /// </summary>
        /// <returns>Returns the updated delivery</returns>
        /// <response code="200"> Successfully updated the delivery </response>
        /// <response code="400"> If the request is incorrect e.g. delivery is null or validation issues </response>
        /// <response code="401"> If the user is not authenticated </response>
        /// <response code="403"> If the user lacks permission to update a delivery </response>
        /// <response code="404"> If there are no delivery with that Id</response>
        /// <response code="500"> If something has gone wrong while updating the list of deliveries </response>
        /// <remarks>
        /// Sample request:
        ///     PUT /deliveries/7432D362-15B3-4CC6-BAB6-F417A5CD5265
        /// </remarks>
        [Authorize(Roles =
            Claims.Partner + "," +
            Claims.Customer + "," +
            Claims.Admin + ",")]
        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Delivery>> PutById(Guid Id, [FromBody] UpdateDelivery newDelivery)
        {
            if (newDelivery is null)
            {
                _logger.LogError("Update delivery request is null");
                return BadRequest();
            }
            var dto = _mapper.Map<UpdateDeliveryDto>(newDelivery);
            dto.Id = Id;
            var response = await _deliveryService.UpdateAsync(dto);
            var result = _mapper.Map<Delivery>(response.Value);
            return HandleResponse(response.Error, result);
        }

        /// <summary>
        /// Updates a List of Deliveries
        /// </summary>
        /// <returns>Returns the updated deliveries</returns>
        /// <response code="200"> Successfully updated some or all the deliveries </response>
        /// <response code="204"> Successful but no deliveries to update </response>
        /// <response code="400"> If the request is incorrect e.g. delivery is null or validation issues </response>
        /// <response code="401"> If the user is not authenticated </response>
        /// <response code="403"> If the user lacks permission to update a delivery </response>
        /// <response code="404"> If there are no delivery with that Id</response>
        /// <response code="500"> If something has gone wrong while updating the list of deliveries </response>
        /// <remarks>
        /// Sample request:
        ///     PUT /deliveries
        /// </remarks>
        [Authorize(Roles =
            Claims.Partner + "," +
            Claims.Admin + ",")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Delivery>> Put([FromBody] List<BulkDelivery> newDelivery)
        {
            if (newDelivery is null)
            {
                _logger.LogError("Update delivery request is null");
                return BadRequest();
            }
            var dto = _mapper.Map<List<UpdateDeliveryDto>>(newDelivery);
            var response = await _deliveryService.BulkUpdateAsync(dto);
            var result = _mapper.Map<List<BulkUpdateResponse>>(response.Value);
            return HandleResponse(response.Error, result);
        }

        /// <summary>
        /// Deletes a Delivery by its Id
        /// </summary>
        /// <returns></returns>
        /// <response code="204"> Successfully deleted the delivery. No content is returned </response>
        /// <response code="401"> If the user is not authenticated </response>
        /// <response code="403"> If the user lacks permission to delete the delivery</response>
        /// <response code="404"> If there are no delivery with that Id</response>
        /// <response code="500"> If something has gone wrong while deleting the delivery </response>
        /// <remarks>
        /// Sample request:
        ///     DELETE /deliveries/7432D362-15B3-4CC6-BAB6-F417A5CD5265
        /// </remarks>
        [Authorize(Roles =
            Claims.Partner + "," +
            Claims.Admin + ",")]
        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteById(Guid Id)
        {
            var response = await _deliveryService.DeleteAsync(Id);
            return HandleResponse(response.Error);
        }

        /// <summary>
        /// Deletes all deliveries
        /// </summary>
        /// <returns></returns>
        /// <response code="204"> Successfully deleted all deliveries. No content is returned </response>
        /// <response code="401"> If the user is not authenticated </response>
        /// <response code="403"> If the user lacks permission to delete all deliveries</response>
        /// <response code="500"> If something has gone wrong while deleting the deliveries </response>
        /// <remarks>
        /// Sample request:
        ///     DELETE /deliveries/7432D362-15B3-4CC6-BAB6-F417A5CD5265
        /// </remarks>
        [Authorize(Roles =
            Claims.Admin + ",")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete()
        {
            var response = await _deliveryService.BulkDeleteAsync();
            return HandleResponse(response.Error);
        }
    }
}
