using global::AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class BaseController : ControllerBase
    {
        internal ActionResult HandleResponse(Errors error) => HandleResponse<object>(error);
        internal ActionResult HandleResponse<T>(Errors error, T data = null, Uri uri = null) where T : class
        {
            return error switch
            {
                Errors.Fail => StatusCode(((int)HttpStatusCode.InternalServerError)),
                Errors.NotFound => NotFound(),
                Errors.BadState => BadRequest(error.GetDescription()),
                Errors.NoAccess => BadRequest(error.GetDescription()),
                Errors.Pass => data is null ? Ok() : Ok(data),
                Errors.NoContent => NoContent(),
                Errors.Created => Created(uri, data),
                _ => throw new System.NotImplementedException()
            };
        }

    }
}
