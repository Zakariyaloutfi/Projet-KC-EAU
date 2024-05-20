using projetkc2.Entities;
using projetkc2.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;


namespace InformationController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InformationController : ControllerBase
    {
        private readonly ApikcContext ApikcContext;

        public InformationController(ApikcContext ApikcContext)
        {
            this.ApikcContext = ApikcContext;
        }

        /// <summary>
        /// Definition du Web Service
        /// </summary>
        /// <remarks>Je manque d'imagination</remarks>
        /// <param name="IdInformation">IdInformation du client a retourné</param>   
        /// <response code="200">client selectionné</response>
        /// <response code="404">client introuvable pour l'IdInformation specifié</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        [HttpGet("GetInformations")]
        public async Task<ActionResult<List<Information>>> Get()
        {
            var List = await ApikcContext.Information.Select(
                s => new Information
                {
                    IdInformation = s.IdInformation,
                    Stades = s.Stades,
                    Kc = s.Kc,
                    Periode = s.Periode,
                    Vergers = s.Vergers,
                
                    Irrigation = s.Irrigation,
                    IdPlante = s.IdPlante
                }
            ).ToListAsync();

            if (List.Count < 0)
            {
                return NotFound();
            }
            else
            {
                return List;
            }
        }

        [HttpGet("GetInformationByIdInformation")]
        public async Task<ActionResult<Information>> GetInformationByIdInformation(int IdInformation)
        {
            Information Information = await ApikcContext.Information.Select(
                    s => new Information
                    {
                        IdInformation = s.IdInformation,
                        Stades = s.Stades,
                        Kc = s.Kc,
                        Periode = s.Periode,
                        Vergers = s.Vergers,
                        Irrigation = s.Irrigation,
                         IdPlante = s.IdPlante
                    })
                .FirstOrDefaultAsync(s => s.IdInformation == IdInformation);

            if (Information == null)
            {
                return NotFound();
            }
            else
            {
                return Information;
            }
        }

        [HttpPost("InsertInformation")]
        public async Task<HttpStatusCode> InsertInformation(Information Information)
        {
            var entity = new Information()
            {
                IdInformation = Information.IdInformation,
                Stades = Information.Stades,
                Kc = Information.Kc,
                Periode = Information.Periode,
                Vergers = Information.Vergers,
                Irrigation = Information.Irrigation,
                 IdPlante = Information.IdPlante
            };

            ApikcContext.Information.Add(entity);
            await ApikcContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        [HttpPut("UpdateInformation")]
        public async Task<HttpStatusCode> UpdateInformation(Information Information)
        {
            var entity = await ApikcContext.Information.FirstOrDefaultAsync(s => s.IdInformation == Information.IdInformation);

            entity.Stades = Information.Stades;
            entity.Kc = Information.Kc;
            entity.Periode = Information.Periode;
            entity.Vergers = Information.Vergers;
            entity.Irrigation = Information.Irrigation;
            entity.IdPlante = Information.IdPlante;

            await ApikcContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpDelete("DeleteInformation/{IdInformation}")]
        public async Task<HttpStatusCode> DeleteInformation(int IdInformation)
        {
            var entity = new Information()
            {
                IdInformation = IdInformation
            };
            ApikcContext.Information.Attach(entity);
            ApikcContext.Information.Remove(entity);
            await ApikcContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}
