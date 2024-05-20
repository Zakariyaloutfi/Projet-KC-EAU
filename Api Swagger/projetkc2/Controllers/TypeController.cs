using projetkc2.Entities;
using projetkc2.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;


namespace projettype.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TypeController : ControllerBase
    {
        private readonly ApikcContext ApikcContext;

        public TypeController(ApikcContext ApikcContext)
        {
            this.ApikcContext = ApikcContext;
        }

        /// <summary>
        /// Definition du Web Service
        /// </summary>
        /// <remarks>Je manque d'imagination</remarks>
        /// <param name="IdType">IdType du client a retourné</param>   
        /// <response code="200">client selectionné</response>
        /// <response code="404">client introuvable pour l'IdType specifié</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        [HttpGet("GetTypess")]
        public async Task<ActionResult<List<Types>>> Get()
        {
            var List = await ApikcContext.Types.Select(
                s => new Types
                {
                    IdType = s.IdType,
                    NomType = s.NomType,


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

        [HttpGet("GetTypesByIdType")]
        public async Task<ActionResult<Types>> GetTypesByIdType(int IdTypes)
        {
            Types Types = await ApikcContext.Types.Select(
                    s => new Types
                    {
                        IdType = s.IdType,
                        NomType = s.NomType,


                    })
                .FirstOrDefaultAsync(s => s.IdType == IdTypes);

            if (Types == null)
            {
                return NotFound();
            }
            else
            {
                return Types;
            }
        }

        [HttpPost("InsertTypes")]
        public async Task<HttpStatusCode> InsertTypes(Types Types)
        {
            var entity = new Types()
            {
                IdType = Types.IdType,
                NomType = Types.NomType,


            };

            ApikcContext.Types.Add(entity);
            await ApikcContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        [HttpPut("UpdateTypes")]
        public async Task<HttpStatusCode> UpdateTypes(Types Types)
        {
            var entity = await ApikcContext.Types.FirstOrDefaultAsync(s => s.IdType == Types.IdType);

            entity.NomType = Types.NomType;



            await ApikcContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpDelete("DeleteTypes/{IdType}")]
        public async Task<HttpStatusCode> DeleteTypes(int IdType)
        {
            var entity = new Types()
            {
                IdType = IdType
            };
            ApikcContext.Types.Attach(entity);
            ApikcContext.Types.Remove(entity);
            await ApikcContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}
