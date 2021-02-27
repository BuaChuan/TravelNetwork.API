using FakeXiecheng.API.Database;
using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.Helper;
using FakeXiecheng.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Services
{
    public class TouristRouteRepository : ITouristRouteRepository
    {
        private readonly AppDbContext _context;
        private IPropertyMappingService _propertyMappingService;

        public TouristRouteRepository(
            AppDbContext appDbContext,
            IPropertyMappingService propertyMappingService
            )
        {
            _context = appDbContext;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutes.Include(t => t.TouristRoutePictures).FirstOrDefaultAsync(n => n.Id == touristRouteId);
        }

        public async Task<PaginationList<TouristRoute>> GetTouristRoutesAsync(
            string keyword, 
            string ratingOperator, 
            int? ratingValue,
            int pageSize,
            int pageNumber,
            string orderBy
        )
        {
            IQueryable<TouristRoute> result = _context
                .TouristRoutes
                .Include(t => t.TouristRoutePictures);
            if(!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                result = result.Where(t => t.Title.Contains(keyword)); 
            }
            int a = pageSize;
            
            if (ratingValue >= 0)
            {
                result = ratingOperator switch
                {
                    "largerThan" => result.Where(t => t.Rating >= ratingValue),
                    "lessThan" => result.Where(t => t.Rating <= ratingValue),
                    _ => result.Where(t => t.Rating == ratingValue),
                };
            }
            /*//pagination
            //跳过数据量
            var skip = (pageNumber - 1) * (pageSize);
            result = result.Skip(skip);
            result = result.Take(pageSize);*/

            
            //动态排序，
            //属性映射服务包含属性映射器集合，
            //属性映射器使用字典描述具体属性映射
            //我们调用服务生成对应的属性映射器，
            var propertyMapper = _propertyMappingService.GetPropertyMapper<TouristRouteDto, TouristRoute>();
            //属性映射器给出映射字典,
            //给排序函数传入排序字符串和映射字典，排序函数完成排序。


            result = result.ApplySort(orderBy, propertyMapper.GetMappingDictionary());
            // include vs join
            return await PaginationList<TouristRoute>.CreateAsync(pageNumber,pageSize,result);
        }

        public async Task<bool> TouristRouteExistsAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutes.AnyAsync(t => t.Id == touristRouteId);
        }

        public async Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutePictures
                .Where(p => p.TouristRouteId == touristRouteId).ToListAsync();
        }

        public async Task<TouristRoutePicture> GetPictureAsync(int pictureId)
        {
            return await _context.TouristRoutePictures.Where(p => p.Id == pictureId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesByIDListAsync(IEnumerable<Guid> ids)
        {
            return await _context.TouristRoutes.Where(t => ids.Contains(t.Id)).ToListAsync();
        }

        public void AddTouristRoute(TouristRoute touristRoute)
        {
            if(touristRoute==null)
            {
                throw new ArgumentNullException(nameof(touristRoute));
            }
            _context.TouristRoutes.Add(touristRoute);
            //_context.SaveChanges();
        }

        public void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture)
        {
            if (touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            if (touristRoutePicture == null)
            {
                throw new ArgumentNullException(nameof(touristRoutePicture));
            }
            touristRoutePicture.TouristRouteId = touristRouteId;
            _context.TouristRoutePictures.Add(touristRoutePicture);
        }

        public void DeleteTouristRoute(TouristRoute touristRoute)
        {
            _context.TouristRoutes.Remove(touristRoute);
        }

        public void DeleteTouristRoutePicture(TouristRoutePicture picture)
        {
            _context.TouristRoutePictures.Remove(picture);
        }

        public void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes)
        {
            _context.TouristRoutes.RemoveRange(touristRoutes);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<ShoppingCart> GetShoppingCartByUserId(string userId)
        {
            return await _context.ShoppingCarts
                .Where(s => s.UserId == userId)
                .Include(s => s.User)              //下面有一个坑，一开始就省略<>会有歧义,识别为ICollection<lineItem>
                .Include(s => s.ShoppingCartItems).ThenInclude<ShoppingCart, LineItem, TouristRoute>(l => l.TouristRoute)
                .FirstOrDefaultAsync();
        }
        public void AddShoppingCart(ShoppingCart shoppingCart)
        {
            _context.ShoppingCarts.Add(shoppingCart);
        }

        public async Task AddShoppingCartItemAsync(LineItem lineItem)
        {
            await _context.LineItems.AddAsync(lineItem);
        }

        public async Task<LineItem> GetShoppingCartItemByItemIdAsync(int itemId)
        {
            return await _context.LineItems.Where(item => item.Id == itemId).FirstOrDefaultAsync();
        }

        public void DeleteShoppingCartItem(LineItem lineItem)
        {
            _context.LineItems.Remove(lineItem);
        }

        public async Task<IEnumerable<LineItem>> GetLineItemsByIdListAsync(IEnumerable<int> itemIds)
        {
            return await _context.LineItems.Where(item => itemIds.Contains(item.Id)).ToListAsync();
        }

        public void DeleteLineItems(IEnumerable<LineItem> lineItems)
        {
            _context.LineItems.RemoveRange(lineItems);
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task<PaginationList<Order>> GetOrdersByUserIdAsync(string userId, int pageSize, int pageNumber)
        {
            var result = _context.Orders.Where(Order => Order.UserId == userId);
            return await PaginationList<Order>.CreateAsync(pageNumber, pageSize, result);
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders
                .Where(order => order.Id == orderId)
                .Include(order => order.OrderItems).ThenInclude((LineItem li) => li.TouristRoute)
                .FirstOrDefaultAsync();
        }
    }
}
