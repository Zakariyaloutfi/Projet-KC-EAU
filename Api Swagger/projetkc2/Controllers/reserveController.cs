using projetkc2.Entities;
using projetkc2.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;


namespace projetIdInformation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class reserveController : ControllerBase
    {
        private readonly ApikcContext ApikcContext;

        public reserveController(ApikcContext ApikcContext)
        {
            this.ApikcContext = ApikcContext;
        }

        /// <summary>
        /// Definition du Web Service
        /// </summary>
        /// <remarks>Je manque d'imagination</remarks>
        /// <param name="IdReserve">IdReserve du client a retourné</param>   
        /// <response code="200">client selectionné</response>
        /// <response code="404">client introuvable pour l'IdReserve specifié</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        [HttpGet("GetReservess")]
        public async Task<ActionResult<List<Reserve>>> Get()
        {
            var List = await ApikcContext.Reserves.Select(
                s => new Reserve
                {
                    IdReserve = s.IdReserve,
                    CodePostale = s.CodePostale,
                    ReserveDeau = s.ReserveDeau

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

        [HttpGet("GetReservesByIdReserve")]
        public async Task<ActionResult<Reserve>> GetReservesByIdReserve(int IdReserves)
        {
            Reserve Reserves = await ApikcContext.Reserves.Select(
                    s => new Reserve
                    {
                        IdReserve = s.IdReserve,
                        CodePostale = s.CodePostale,
                        ReserveDeau = s.ReserveDeau


                    })
                .FirstOrDefaultAsync(s => s.IdReserve == IdReserves);

            if (Reserves == null)
            {
                return NotFound();
            }
            else
            {
                return Reserves;
            }
        }

        [HttpPost("InsertReserves")]
        public async Task<HttpStatusCode> InsertReserves(Reserve Reserves)
        {
            var entity = new Reserve()
            {
                IdReserve = Reserves.IdReserve,
                CodePostale = Reserves.CodePostale,
                ReserveDeau = Reserves.ReserveDeau


            };

            ApikcContext.Reserves.Add(entity);
            await ApikcContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        [HttpPut("UpdateReserves")]
        public async Task<HttpStatusCode> UpdateReserves(Reserve Reserves)
        {
            var entity = await ApikcContext.Reserves.FirstOrDefaultAsync(s => s.IdReserve == Reserves.IdReserve);

            entity.CodePostale = Reserves.CodePostale;
            entity.ReserveDeau = Reserves.ReserveDeau;



            await ApikcContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpDelete("DeleteReserves/{IdReserve}")]
        public async Task<HttpStatusCode> DeleteReserves(int IdReserve)
        {
            var entity = new Reserve()
            {
                IdReserve = IdReserve
            };
            ApikcContext.Reserves.Attach(entity);
            ApikcContext.Reserves.Remove(entity);
            await ApikcContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
        [HttpGet("CalculateAutonomie")]
        public ActionResult<double> CalculateAutonomie(double kc, double volumeEau, double surfaceCulture)
        {
            double ETRef = 5.0; 
            double ETc = kc * ETRef; 
            double consommationParJour = ETc * surfaceCulture / 1000; 
            double autonomie = volumeEau / consommationParJour; 

            return Ok(autonomie);
        }

    }
}
