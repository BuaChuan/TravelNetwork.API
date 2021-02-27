using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Text.RegularExpressions;
using FakeXiecheng.API.ResourceParameters;
using FakeXiecheng.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using FakeXiecheng.API.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Diagnostics.Eventing.Reader;

namespace FakeXiecheng.API.Controllers
{
    [Route("api/[controller]")] // api/touristroute
    [ApiController]
    public class TouristRoutesController : ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;
        //private readonly IUrlHelper _urlHelper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IShapeDataService _shapeDataService;
        
        public TouristRoutesController(
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper,
         //   IUrlHelperFactory urlHelperFactory,
       //     IActionContextAccessor actionContextAccessor,
            IPropertyMappingService propertyMappingService,
            IShapeDataService shapeDataService
        )
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
           // _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _propertyMappingService = propertyMappingService;
            _shapeDataService = shapeDataService;
            
        }

        //返回一个绝对URL
        private string GenerateTouristRouteResourceUrl(
             TouristRouteResourceParameters parameters0,
             PaginationResourceParameters parameters1,
             ResourceUrlType type
            )
        {
            return type switch
            {
                ResourceUrlType.NextPage => Url.Link("GetTouristRoutes",
                new
                {
                    fields = parameters0.Fields,
                    orderBy = parameters0.OrderBy,
                    keyword = parameters0.Keyword,
                    rating = parameters0.Rating,
                    pageNumber = parameters1.PageNumber + 1,
                    pageSize = parameters1.PageSize
                }),
                ResourceUrlType.PreviousPage => Url.Link("GetTouristRoutes",
                new
                {
                    fields = parameters0.Fields,
                    orderBy = parameters0.OrderBy,
                    keyword = parameters0.Keyword,
                    rating = parameters0.Rating,
                    pageNumber = parameters1.PageNumber - 1,
                    pageSize = parameters1.PageSize
                }),
                _ => Url.Link("GetTouristRoutes",
                new
                {
                    fields = parameters0.Fields,
                    orderBy = parameters0.OrderBy,
                    keyword = parameters0.Keyword,
                    rating = parameters0.Rating,
                    pageNumber = parameters1.PageNumber,
                    pageSize = parameters1.PageSize
                })

                /*ResourceUrlType.NextPage => _urlHelper.Link("GetTouristRoutes",
                new
                {
                    fields = parameters0.Fields,
                    orderBy = parameters0.OrderBy,
                    keyword = parameters0.Keyword,
                    rating = parameters0.Rating,
                    pageNumber = parameters1.PageNumber + 1,
                    pageSize = parameters1.PageSize
                }),
                ResourceUrlType.PreviousPage => _urlHelper.Link("GetTouristRoutes",
                new
                {
                    fields = parameters0.Fields,
                    orderBy = parameters0.OrderBy,
                    keyword = parameters0.Keyword,
                    rating = parameters0.Rating,
                    pageNumber = parameters1.PageNumber - 1,
                    pageSize = parameters1.PageSize
                }),
                _ => _urlHelper.Link("GetTouristRoutes",
                new
                {
                    fields = parameters0.Fields,
                    orderBy = parameters0.OrderBy,
                    keyword = parameters0.Keyword,
                    rating = parameters0.Rating,
                    pageNumber = parameters1.PageNumber,
                    pageSize = parameters1.PageSize
                })*/
            };
        }

        // api/touristRoutes?keyword=传入的参数
        [HttpGet(Name = "GetTouristRoutes")]
        [HttpHead]
        public async Task<IActionResult> GetTouristRoutes(
            [FromQuery] TouristRouteResourceParameters parameters0,
            [FromQuery] PaginationResourceParameters parameters1
        //[FromQuery] string keyword,
        //string rating // 小于lessThan, 大于largerThan, 等于equalTo lessThan3, largerThan2, equalTo5 
        )// FromQuery vs FromBody
        {
            if (parameters0.Fields != null && 
                !_shapeDataService.FieldsIsValid<TouristRouteDto>(parameters0.Fields))
            {
                return BadRequest("请输入正确的Fields");
            }
            if(
                parameters0.OrderBy != null && 
                !_propertyMappingService.IsMappingExists<TouristRouteDto, TouristRoute>(parameters0.OrderBy)
            )
            {
                return BadRequest("请输入正确的OrderBy");
            }
            var touristRoutesFromRepo = await _touristRouteRepository.GetTouristRoutesAsync(
                parameters0.Keyword, 
                parameters0.RatingOperator, 
                parameters0.RatingValue,
                parameters1.PageSize,
                parameters1.PageNumber,
                parameters0.OrderBy
            );
            if (touristRoutesFromRepo == null || touristRoutesFromRepo.Count() <= 0)
            {
                return NotFound("没有旅游路线");
            }
            var touristRoutesDto = _mapper.Map<IEnumerable<TouristRouteDto>>(touristRoutesFromRepo);
            var nextPageLink = touristRoutesFromRepo.HasNext ?
                GenerateTouristRouteResourceUrl(parameters0, parameters1, ResourceUrlType.NextPage) : null;
            var previousPageLink = touristRoutesFromRepo.HasPrevious ?
                GenerateTouristRouteResourceUrl(parameters0, parameters1, ResourceUrlType.PreviousPage) : null;
            //x-pagination
            var paginationMetadata = new
            {
                previousPageLink,
                nextPageLink,
                totalCount = touristRoutesFromRepo.TotalCount,
                pageSize = touristRoutesFromRepo.PageSize,
                currentPage = touristRoutesFromRepo.CurrentPage,
                totalPages = touristRoutesFromRepo.TotalPages
            };
            HttpContext.Response.Headers.Add("x-pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));
            return Ok(_shapeDataService.ShapeMultipleData(parameters0.Fields, touristRoutesDto));
        }


        private IEnumerable<LinkDto> CreateLinkForTouristRoute(
            Guid touristRouteId,
            string fields)
        {
            var links = new List<LinkDto>();
            links.Add(new LinkDto(
                href: Url.Link("GetTouristRouteById", new { touristRouteId, fields }),
                rel: "self",
                method: "GET"
                ));
            links.Add(new LinkDto(
                href: Url.Link("UpdateTouristRoute", new { touristRouteId}),
                rel: "update_touristRoute",
                method: "PUT"
                ));
            links.Add(new LinkDto(
               href: Url.Link("PartiallyUpdateTouristRoute", new { touristRouteId }),
               rel: "partially_update_touristRoute",
               method: "PATCH"
               ));
            links.Add(new LinkDto(
               href: Url.Link("DeleteTouristRoute", new { touristRouteId }),
               rel: "delete_touristRoute",
               method: "DELETE"
               ));
            links.Add(new LinkDto(
               href: Url.Link("CreateTouristRoutePicture", new { touristRouteId }),
               rel: "create_picture",
               method: "DELETE"
               ));
            links.Add(new LinkDto(
               href: Url.Link("GetPictureListForTouristRoute", new { touristRouteId }),
               rel: "get_pictures",
               method: "DELETE"
               ));
            return links;
        }
        // api/touristroutes/{touristRouteId}
        [HttpGet("{touristRouteId}", Name = "GetTouristRouteById")]
        public async Task<IActionResult> GetTouristRouteById(
            Guid touristRouteId,
            [FromQuery] string fields
            )
        {
            if (fields != null &&
                !_shapeDataService.FieldsIsValid<TouristRouteDto>(fields))
            {
                return BadRequest("请输入正确的Fields");
            }
            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            if (touristRouteFromRepo == null)
            {
                return NotFound($"旅游路线{touristRouteId}找不到");
            }
            //var touristRouteDto = new TouristRouteDto()
            //{
            //    Id = touristRouteFromRepo.Id,
            //    Title = touristRouteFromRepo.Title,
            //    Description = touristRouteFromRepo.Description,
            //    Price = touristRouteFromRepo.OriginalPrice * (decimal)(touristRouteFromRepo.DiscountPresent ?? 1),
            //    CreateTime = touristRouteFromRepo.CreateTime,
            //    UpdateTime = touristRouteFromRepo.UpdateTime,
            //    Features = touristRouteFromRepo.Features,
            //    Fees = touristRouteFromRepo.Fees,
            //    Notes = touristRouteFromRepo.Notes,
            //    Rating = touristRouteFromRepo.Rating,
            //    TravelDays = touristRouteFromRepo.TravelDays.ToString(),
            //    TripType = touristRouteFromRepo.TripType.ToString(),
            //    DepartureCity = touristRouteFromRepo.DepartureCity.ToString()
            //};
            var touristRouteDto = _mapper.Map<TouristRouteDto>(touristRouteFromRepo);
            var linkDtos = CreateLinkForTouristRoute(touristRouteId, fields);
            var result = _shapeDataService.ShapeSingleData(fields, touristRouteDto)
                as IDictionary<string, object>;
            result.Add("link", linkDtos);
            return Ok(result);

            
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize]
        public async Task<IActionResult> CreateTouristRoute([FromBody] TouristRouteForCreationDto touristRouteForCreationDto)
        {
            var touristRouteModel = _mapper.Map<TouristRoute>(touristRouteForCreationDto);
            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            await _touristRouteRepository.SaveAsync();
            var touristRouteToReture = _mapper.Map<TouristRouteDto>(touristRouteModel);
            return CreatedAtRoute(
                "GetTouristRouteById",
                new { touristRouteId = touristRouteToReture.Id },
                touristRouteToReture
            );
        }

        [HttpPut("{touristRouteId}", Name = "UpdateTouristRoute")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTouristRoute(
            [FromRoute]Guid touristRouteId,
            [FromBody] TouristRouteForUpdateDto touristRouteForUpdateDto
        )
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("旅游路线找不到");
            }

            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            // 1. 映射dto
            // 2. 更新dto
            // 3. 映射model
            _mapper.Map(touristRouteForUpdateDto, touristRouteFromRepo);

            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

        //局部更新
        [HttpPatch("{touristRouteId}",Name = "PartiallyUpdateTouristRoute")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PartiallyUpdateTouristRoute(
            [FromRoute]Guid touristRouteId,
            [FromBody] JsonPatchDocument<TouristRouteForUpdateDto> patchDocument
        )
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("旅游路线找不到");
            }

            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            var touristRouteToPatch = _mapper.Map<TouristRouteForUpdateDto>(touristRouteFromRepo);
            patchDocument.ApplyTo(touristRouteToPatch, ModelState);
            if (!TryValidateModel(touristRouteToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(touristRouteToPatch, touristRouteFromRepo);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

        //删除单条旅游路线
        [HttpDelete("{touristRouteId}", Name = "DeleteTouristRoute")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTouristRoute([FromRoute] Guid touristRouteId)
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("旅游路线找不到");
            }

            var touristRoute = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            _touristRouteRepository.DeleteTouristRoute(touristRoute);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("({touristIDs})")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteByIDs(
            [ModelBinder( BinderType = typeof(ArrayModelBinder))][FromRoute]IEnumerable<Guid> touristIDs)
        {
            if(touristIDs == null)
            {
                return BadRequest();
            }

            var touristRoutesFromRepo = await _touristRouteRepository.GetTouristRoutesByIDListAsync(touristIDs);
            _touristRouteRepository.DeleteTouristRoutes(touristRoutesFromRepo);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }
    }
}