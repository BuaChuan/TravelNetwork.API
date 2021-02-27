using AutoMapper;
using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.Helper;
using FakeXiecheng.API.Models;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Controllers
{
    [ApiController]
    [Route("api/shoppingCart")]
    public class ShoppingCartController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        public ShoppingCartController(
            UserManager<ApplicationUser> userManager,
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper
         //   ,IHttpContextAccessor httpContextAccessor
            )
        {
            _userManager = userManager;
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
           // _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetShoppingCartAsync()
        {
            //1获取当前用户
           // var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.GetUserAsync(HttpContext.User); var userId = user.Id;

           // var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;  
            //2使用userid获取购物车
            var shoppingCartByRepo = await _touristRouteRepository.GetShoppingCartByUserId(userId);
            return Ok(_mapper.Map<ShoppingCartDto>(shoppingCartByRepo));
             
        }
        [HttpPost("items")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddShopingCartItemAsync([FromBody] AddShoppingCartItemDto addShoppingCartItemDto)
        {
            //1获取当前用户
            // var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.GetUserAsync(HttpContext.User); var userId = user.Id;

            // var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;  

            //2使用userid获取购物车
            var shoppingCartByRepo = await _touristRouteRepository.GetShoppingCartByUserId(userId);

            //3
            var touristRoute = await _touristRouteRepository.GetTouristRouteAsync(addShoppingCartItemDto.TouristRouteId);
            if(touristRoute == null)
            {
                return NotFound("旅游路线不存在");
            }
            var lineItem = new LineItem()
            {
                TouristRouteId = addShoppingCartItemDto.TouristRouteId,
                ShoppingCartId = shoppingCartByRepo.Id,
                OriginalPrice = touristRoute.OriginalPrice,
                DiscountPresent = touristRoute.DiscountPresent

            };
            await _touristRouteRepository.AddShoppingCartItemAsync(lineItem);
            await _touristRouteRepository.SaveAsync();
            return Ok(_mapper.Map<ShoppingCartDto>(shoppingCartByRepo));
        }
        [HttpDelete("items/{itemId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteShoppingCartItemAsync([FromRoute] int itemId)
        {
            var lineItemByRepo = await _touristRouteRepository.GetShoppingCartItemByItemIdAsync(itemId);
            if(lineItemByRepo == null)
            {
                return NotFound("未找到购物车商品");
            }
            _touristRouteRepository.DeleteShoppingCartItem(lineItemByRepo);
            await _touristRouteRepository.SaveAsync();
            return NoContent();
        }
        
        [HttpDelete("items/({itemIds})")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteShoppingCartItemAsync(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))][FromRoute] IEnumerable<int> itemIds)
        {
            if(itemIds == null)
            {
                return BadRequest();
            }
            var lineItems = await _touristRouteRepository.GetLineItemsByIdListAsync(itemIds);
            _touristRouteRepository.DeleteLineItems(lineItems);
            await _touristRouteRepository.SaveAsync();
            return NoContent();
        }
        [HttpPost("checkout")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CheckoutAsync()
        {
            //1获取当前用户
            // var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.GetUserAsync(HttpContext.User); var userId = user.Id;

            // var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;  

            //2使用userid获取购物车
            var shoppingCartByRepo = await _touristRouteRepository.GetShoppingCartByUserId(userId);
            var order = new Order()
            {
                Id = new Guid(),
                UserId = userId,
                OrderItems = shoppingCartByRepo.ShoppingCartItems,
                State = OrderStateEnum.Pending,
                CreateDateUTC = DateTime.Now
                
            };
            shoppingCartByRepo.ShoppingCartItems = null;
            await _touristRouteRepository.AddOrderAsync(order);
            await _touristRouteRepository.SaveAsync();
            return Ok(_mapper.Map<OrderDto>(order));
        }
    }
}
