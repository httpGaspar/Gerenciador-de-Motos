using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; //biblioteca de framework para interagir com o banco de dados

//referenciam os arquivos de contexto do banco de dados (motocontext e moto)
using MotoApi.Data; 
using MotoApi.Models;

namespace MotoApi.Controllers //define onde o controller está
{
    [Route("api/[controller]")] // define a rota da API (api/Mmtos)
    [ApiController] //marca a classe como controller de API
    public class MotosController : ControllerBase{ //classe de controller
        private readonly MotoContext _context; //dependência para acessar as motos no banco

        // coloca o contexto de banco de dados na classe do controlador
        // permite a manipulação das motos no banco de dados.
        public MotosController(MotoContext context) 
        {
            _context = context; 
        }

        [HttpPost] //metodo POST
        public async Task<IActionResult> CreateMoto([FromBody] Moto moto) //recebe uma nova moto
        {
           if (!ModelState.IsValid) // Verifica se o modelo recebido é válido
           {
            return BadRequest(ModelState); //retorna um erro caso o modelo seja invalido
           }
            
           _context.Motos.Add(moto); //adiciona a moto cadastrada ao banco
           await _context.SaveChangesAsync(); //salva as alterações

            return CreatedAtAction(nameof(GetAllMotos), new { }, moto); //retorna 201
        }
        

        [HttpGet] //metodo get geral
        public async Task<ActionResult<IEnumerable<Moto>>> GetAllMotos()
        {
            return await _context.Motos.ToListAsync(); //retorna a lista das motos
        }


        [HttpGet("{placa}")] //metodo get que filtra pela placa
        public async Task<ActionResult<Moto>> GetMotoByPlaca(string placa)
        {
            //busca uma moto que tenha a placa expecificada
            var moto = await _context.Motos
                .Where(m => m.Placa == placa)
                .FirstOrDefaultAsync();

            if (moto == null)
            {
                return NotFound(); //retorna um erro caso a moto não seja encontradd
            }

            return moto; //retorna a moto encontrada
        }


        [HttpPut("{id}")] //metoo put para atualizar os dados da moto
        public async Task<ActionResult<Moto>> UpdateMoto(Guid id, Moto moto)
        {
            //verifica se o id passado na url é o mesmo da moto
            if (id != moto.Id)
            {
                return BadRequest(); //erro 400 
            }

            _context.Entry(moto).State = EntityState.Modified; //marca o estado da moto como modificado
            await _context.SaveChangesAsync(); //salva as alterações

            return Ok(moto); //retorna 200
        }


        [HttpDelete("{id}")] //metodo para deletar 
        public async Task<ActionResult<Moto>> DeleteMoto(Guid id)
        {
            var moto = await _context.Motos.FindAsync(id); //busca a moto pelo Id
            if (moto == null) //caso não encontre a moto retorna um erro 404
            {
                return NotFound();
            }

            _context.Motos.Remove(moto); //remove a moto do banco
            await _context.SaveChangesAsync(); //salva as alterações

            return NoContent(); //retorna 204 (operação bem sucedida)
        }
    }
}