using AutoMapper;
using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(
                   dest => dest.State,
                   option =>
                   {
                       option.MapFrom(source => source.State.ToString());
                   }
                );
        }
    }
}
