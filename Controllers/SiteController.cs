using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using winetranet_api.DTO;
using winetranet_api.Entities;

namespace winetranet_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {
        private readonly WinetranetContext _context;
        private readonly DbSet<Site> _sites;
        private readonly IMapper _mapper;
        public SiteController(WinetranetContext context, IMapper mapper)
        {
            _context = context;
            _sites = _context.Sites;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("GetSites")]
        public async Task<ActionResult<List<SiteDTO>>> GetSites()
        {
            return Ok(_sites.Select(site => _mapper.Map<SiteDTO>(site)).ToList().OrderBy(x => x.Id));
        }

        [Authorize]
        [HttpGet("GetSiteById")]
        public async Task<ActionResult<SiteDTO>> GetSiteById(int id)
        {
            SiteDTO? Site = _mapper.Map<SiteDTO>(_sites.FirstOrDefault(x => x.Id == id));

            return Site == null ? NotFound() : Ok(Site);
        }

        [Authorize]
        [HttpPut("UpdateSite")]
        public async Task<ActionResult<SiteDTO>> UpdateSite(SiteDTO siteToUpdate)
        {
            var site = await _sites.FirstOrDefaultAsync(x => x.Id == siteToUpdate.Id);

            if (site == null)
            {
                Site siteMapped = _mapper.Map<Site>(siteToUpdate);
                siteMapped.Id = siteToUpdate.Id;
                siteMapped.Ville = siteToUpdate.Ville;

                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<SiteDTO>(siteMapped));
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost("addSite")]
        public async Task<ActionResult<SiteDTO>> CreateSite(String siteVilleToCreate)
        {

            if (siteVilleToCreate != null)
            {
                if (_sites.Any(x => x.Ville == siteVilleToCreate))
                {
                    return BadRequest("Site already exists with this town.");
                }
                var site = new Site();
                site.Ville = siteVilleToCreate;
                
                _sites.Add(site);
                await _context.SaveChangesAsync();
                return Ok(_mapper.Map<SiteDTO>(site));
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpDelete("deleteSite")]
        public async Task<ActionResult<SiteDTO>> DeleteSite(int siteIdToDelete)
        {
            var site = await _sites.FirstOrDefaultAsync(x => x.Id == siteIdToDelete);

            if (site != null)
            {
                _sites.Remove(site);
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
