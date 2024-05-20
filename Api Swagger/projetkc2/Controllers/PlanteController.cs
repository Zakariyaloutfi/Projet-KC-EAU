using projetkc2.Entities;
using projetkc2.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;


namespace projetIdInformation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanteController : ControllerBase
    {
        private readonly ApikcContext ApikcContext;

        public PlanteController(ApikcContext ApikcContext)
        {
            this.ApikcContext = ApikcContext;
        }

        /// <summary>
        /// Definition du Web Service
        /// </summary>
        /// <remarks>Je manque d'imagination</remarks>
        /// <param name="IdPlante">IdPlante du client a retourné</param>   
        /// <response code="200">client selectionné</response>
        /// <response code="404">client introuvable pour l'IdPlante specifié</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        [HttpGet("GetPlantess")]
        public async Task<ActionResult<List<Plante>>> Get()
        {
            var List = await ApikcContext.Plantes.Select(
                s => new Plante
                {
                    IdPlante = s.IdPlante,
                    NomPlante = s.NomPlante,
                    IdType = s.IdType
                   
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

        [HttpGet("GetPlantesByIdPlante")]
        public async Task<ActionResult<Plante>> GetPlantesByIdPlante(int IdPlantes)
        {
            Plante Plantes = await ApikcContext.Plantes.Select(
                    s => new Plante
                    {
                        IdPlante = s.IdPlante,
                        NomPlante = s.NomPlante,
                        IdType = s.IdType


                    })
                .FirstOrDefaultAsync(s => s.IdPlante == IdPlantes);

            if (Plantes == null)
            {
                return NotFound();
            }
            else
            {
                return Plantes;
            }
        }

        [HttpPost("InsertPlantes")]
        public async Task<HttpStatusCode> InsertPlantes(Plante Plantes)
        {
            var entity = new Plante()
            {
                IdPlante = Plantes.IdPlante,
                NomPlante = Plantes.NomPlante,
                IdType = Plantes.IdType


            };

            ApikcContext.Plantes.Add(entity);
            await ApikcContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        [HttpPut("UpdatePlantes")]
        public async Task<HttpStatusCode> UpdatePlantes(Plante Plantes)
        {
            var entity = await ApikcContext.Plantes.FirstOrDefaultAsync(s => s.IdPlante == Plantes.IdPlante);

            entity.NomPlante = Plantes.NomPlante;
            entity.IdType = Plantes.IdType;



            await ApikcContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpDelete("DeletePlantes/{IdPlante}")]
        public async Task<HttpStatusCode> DeletePlantes(int IdPlante)
        {
            var entity = new Plante()
            {
                IdPlante = IdPlante
            };
            ApikcContext.Plantes.Attach(entity);
            ApikcContext.Plantes.Remove(entity);
            await ApikcContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}
    