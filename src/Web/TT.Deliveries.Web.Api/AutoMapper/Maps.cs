using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TT.Deliveries.Data.Dto;
using Entities = TT.Deliveries.Data.Entities;
using ApiModels = TT.Deliveries.Web.Api.Models;
using TT.Deliveries.Services.Responses;
using TT.Deliveries.Core.Extensions;

namespace TT.Deliveries.Web.Api.AutoMapper
{
    public class Maps : Profile
    {
        public Maps()
        {
            //Map entities to Dtos
            CreateMap<Entities.Delivery, CreateDeliveryDto>().ReverseMap();
            CreateMap<Entities.Delivery, UpdateDeliveryDto>().ReverseMap();

            CreateMap<Entities.Delivery, DeliveryDto>().ReverseMap();
            CreateMap<Entities.Recipient, RecipientDto>().ReverseMap();
            CreateMap<Entities.AccessWindow, AccessWindowDto>().ReverseMap();
            CreateMap<Entities.Order, OrderDto>().ReverseMap();

            //Map Api Models to Dtos
            CreateMap<ApiModels.CreateDelivery, CreateDeliveryDto>();
            CreateMap<ApiModels.UpdateDelivery, UpdateDeliveryDto>();
            CreateMap<ApiModels.BulkDelivery, UpdateDeliveryDto>();
            CreateMap<ApiModels.BulkDelivery, DeliveryDto>().ReverseMap();

            CreateMap<ApiModels.Delivery, DeliveryDto>().ReverseMap();
            CreateMap<ApiModels.Recipient, RecipientDto>().ReverseMap();
            CreateMap<ApiModels.AccessWindow, AccessWindowDto>().ReverseMap();
            CreateMap<ApiModels.Order, OrderDto>().ReverseMap();

            //Entity to Api models
            //CreateMap<Entities.Delivery, ApiModels.Delivery>().ReverseMap();
            //CreateMap<Entities.Delivery, ApiModels.BulkDelivery>().ReverseMap();

            //CreateMap<Entities.Recipient, ApiModels.Recipient>().ReverseMap();
            //CreateMap<Entities.AccessWindow, ApiModels.AccessWindow>().ReverseMap();
            //CreateMap<Entities.Order, ApiModels.Order>().ReverseMap();

            //Map Error code to Bulk update status code
            CreateMap<Response<Guid>, ApiModels.BulkUpdateResponse>()
                .ForMember(
                    member => member.Id,
                    x => x.MapFrom(
                        res => res.Value)
                )
                .ForMember(
                    member => member.Status,
                    x => x.MapFrom(
                        res => res.Error.GetDescription())
                );
        }
    }
}
