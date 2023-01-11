using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using winetranet_api.DTO;
using winetranet_api.Entities;

namespace winetranet_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly WinetranetContext _context;
        private readonly DbSet<Service> _services;
        private readonly IMapper _mapper;
        public ServiceController(WinetranetContext context, IMapper mapper)
        {
            _context = context;
            _services = _context.Services;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("GetServices")]
        public async Task<ActionResult<List<ServiceDTO>>> GetServices()
        {
            return Ok(_services.Select(Service => _mapper.Map<ServiceDTO>(Service)).ToList().OrderBy(x => x.Id));
        }

        [Authorize]
        [HttpGet("GetServiceById")]
        public async Task<ActionResult<ServiceDTO>> GetServiceById(int id)
        {
            ServiceDTO? Service = _mapper.Map<ServiceDTO>(_services.FirstOrDefault(x => x.Id == id));

            return Service == null ? NotFound() : Ok(Service);
        }

        [Authorize]
        [HttpPut("UpdateService")]
        public async Task<ActionResult<ServiceDTO>> UpdateService(ServiceDTO serviceToUpdate)
        {
            var Service = await _services.FirstOrDefaultAsync(x => x.Id == serviceToUpdate.Id);

            if (Service == null)
            {
                Service serviceMapped = _mapper.Map<Service>(serviceToUpdate);
                serviceMapped.Id = serviceToUpdate.Id;
                serviceMapped.Name = serviceToUpdate.Name;

                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<ServiceDTO>(serviceMapped));
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost("addService")]
        public async Task<ActionResult<ServiceDTO>> CreateService(String serviceNameToCreate)
        {
            if (serviceNameToCreate != null)
            {
                if (_services.Any(x => x.Name == serviceNameToCreate))
                {
                    return BadRequest("Service already exists with this name.");
                }
                var service = new Service();

                service.Name = serviceNameToCreate;

                _services.Add(service);
                await _context.SaveChangesAsync();
                return Ok(_mapper.Map<ServiceDTO>(service));
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpDelete("deleteService")]
        public async Task<ActionResult<ServiceDTO>> DeleteService(int serviceId)
        {
            var service = await _services.FirstOrDefaultAsync(x => x.Id == serviceId);

            if (service != null)
            {
                _services.Remove(service);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
