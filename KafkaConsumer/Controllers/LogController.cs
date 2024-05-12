using Application.MongoDB.Log;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KafkaConsumer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogRepository _repository;

        public LogController(ILogRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<List<LogEntity>> Get() 
        {
            var res = await _repository.GetAllAsync();
            return res.ToList();
        }
    }
}
