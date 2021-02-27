using AutoMapper;
using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.Models;
using FakeXiecheng.API.ResourceParameters;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private ITouristRouteRepository _touristRouteRepository;
        private IMapper _mapper;
        public OrdersController(
            UserManager<ApplicationUser> userManager,
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper
        )
        {
            _userManager = userManager;
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrdersAync(
            [FromQuery] PaginationResourceParameters parameters
        )
        {
            //1获取当前用户
            // var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.GetUserAsync(HttpContext.User); var userId = user.Id;

            // var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;  

            //2使用userid获取用户订单记录（不包含详细信息，订单详细信息通过订单id查询）
            var orders =await _touristRouteRepository.GetOrdersByUserIdAsync(userId, parameters.PageSize, parameters.PageNumber);
            return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
        }

        //教程此处有问题：任意用户登录后，可以通过订单Id查询任意订单，按正确逻辑查询范围应该限制为自己的订单
        [HttpGet("{orderId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrderByIdAsync([FromRoute]Guid orderId)
        {
            var order = await _touristRouteRepository.GetOrderByIdAsync(orderId);
            return Ok(_mapper.Map<OrderDto>(order));
            
        }
    }
}
