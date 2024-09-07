using API.Controllers.Base;
using Application.DTOs.Request;
using Application.Extentions;
using Application.Services;
using Domain.Entity.Products;
using Infrastructure.Data;
using Infrastructure.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestEase;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController<Guid, Product>, IProduct
    {
        readonly Repository _repository;

        public ProductsController(Repository repository = null) : base(repository.SProduct)
        {
            _repository = repository;
        }

        [HttpPost(ApiRoutes.Product.GetFillter)]
        public Task<Result<List<Product>>> GetFilter([Body] Fillter model)
        {
            throw new NotImplementedException();
        }
    }
}
