using FakeXiecheng.API.Helper;
using FakeXiecheng.API.Models;
using FakeXiecheng.API.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Services
{
    public interface ITouristRouteRepository
    {
        Task<PaginationList<TouristRoute>> GetTouristRoutesAsync(string keyword, string ratingOperator, int? ratingValue, int pageSize, int pageNumber, string orderBy);

        Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId);
        Task<bool> TouristRouteExistsAsync(Guid touristRouteId);
        Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid touristRouteId);
        Task<TouristRoutePicture> GetPictureAsync(int pictureId);
        Task<IEnumerable<TouristRoute>> GetTouristRoutesByIDListAsync(IEnumerable<Guid> ids);
        void AddTouristRoute(TouristRoute touristRoute);
        void AddShoppingCart(ShoppingCart shoppingCart);
        void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture);
        void DeleteTouristRoute(TouristRoute touristRoute);
        void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes);
        void DeleteTouristRoutePicture(TouristRoutePicture picture);
        Task<ShoppingCart> GetShoppingCartByUserId(string userId);
        Task AddShoppingCartItemAsync(LineItem lineItem);
        Task<LineItem> GetShoppingCartItemByItemIdAsync(int itemId);
        void DeleteShoppingCartItem(LineItem lineItem);
        Task<IEnumerable<LineItem>> GetLineItemsByIdListAsync(IEnumerable<int> itemIds);
        void DeleteLineItems(IEnumerable<LineItem> lineItems);
        
        Task<PaginationList<Order>> GetOrdersByUserIdAsync(string userId, int pageSize, int pageNumber);
        Task AddOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task<bool> SaveAsync();
    }
}
