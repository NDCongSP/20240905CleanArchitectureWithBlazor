﻿using Application.Extentions;
using Application.Services.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestEase;

namespace API.Controllers.Base
{
    //[Authorize]//không cần xét role, login vào là gọi đc API    
    [Route("api/[controller]")]
    [ApiController]
    //[ApiController,JsonifyErrors]
    public class BaseController<TId, T> : ControllerBase, IRepository<TId, T> where T : class
    {
        readonly IRepository<TId, T> _repository;

        public BaseController(IRepository<TId, T> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public Task<Result<List<T>>> GetAll()
        {
            return _repository.GetAll();
        }

        [HttpGet(ApiRoutes.GetById)]
        public Task<Result<T>> GetById([Path] TId id)
        {
            return _repository.GetById(id);
        }

        [HttpPost(ApiRoutes.Insert)]
        public Task<Result<T>> Insert([Body] T model)
        {
            return _repository.Insert(model);
        }

        [HttpPost(ApiRoutes.Update)]
        public Task<Result<T>> Update([Body] T model)
        {
            return _repository.Update(model);
        }
    }
}