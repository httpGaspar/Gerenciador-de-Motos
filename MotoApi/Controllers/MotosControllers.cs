using Microsoft.AspNetCore.Mvc; //importa funcionalidades para criar controladores e ações
using Microsoft.EntityFrameworkCore; //biblioteca de framework para interagir com o banco de dados

//referenciam o contexto do banco de dados (motocontext e moto)
using MotoApi.Data; 
using MotoApi.Models;

namespace MotoApi.Controllers //define onde o controller está
{
    [ApiController] //marca a classe como controller de API
    [Route("api/[controller]")] // define a rota da API (api/Mmtos)
    public class MotosController : ControllerBase{ //classe de controller
        private readonly MotoContext _context; //dependência para acessar as motos no banco

        // injeta o contexto de banco de dados na classe do controlador via injeção de dependência.
        // permite a manipulação das motos no banco de dados.
        public MotosController(MotoContext context) 
        {
            _context = context; 
        }

        [HttpPost] //metodo POST
        public async Task<IActionResult> CreateMoto([FromBody] Moto moto) //recebe uma nova moto
        {
            //verifica se a moto já está cadastrada e retorna um erro se sim
            if (_context.Motos.Any(m => m.Placa == moto.Placa))
            {
                return BadRequest("Placa já cadastrada.");
            }
            
            moto.Id = Guid.NewGuid(); ////atribui um ID único a moto
            _context.Motos.Add(moto); //adiciona a nova moto no banco
            await _context.SaveChangesAsync(); //salva a alteração 

            return Ok(moto); //retorna 200 OK
        }

        [HttpGet] //metodo GET (buscar)
        public async Task<IActionResult> GetMotos([FromQuery] string placa) //recebe o parametro de consulta e filtra por placa
        {
            var query = _context.Motos.AsQueryable(); //cria uma consulta no banco de dados

            //verifica se o parametro da placa foi informado
            if (!string.IsNullOrEmpty(placa))
            {
                query = query.Where(m => m.Placa == placa); //filtra as motos pela placa
            }

            return Ok(await query.ToListAsync()); //retorna a lista das motos 200 OK
        }

        [HttpPut("{id}")] //metodo Put (atualizar) o id tem que ser passado via URL
        public async Task<IActionResult> UpdateMoto(Guid id, [FromBody] string novaPlaca) //recebe o id da moto e a nova placa na requisição
        {
            var moto = await _context.Motos.FindAsync(id); //busca a moto pelo id
            if (moto == null) return NotFound();

             // Verifica se a nova placa já está cadastrada
            if (_context.Motos.Any(m => m.Placa == novaPlaca))
            {
                return BadRequest("Placa já cadastrada.");
            }

            moto.Placa = novaPlaca; //atualiza a placa da moto
            await _context.SaveChangesAsync(); //salva a alteração

            return NoContent(); //avisa que a operação foi bem sucedida 204 no content
        }

       [HttpDelete("{id}")] // Método de deletar
        public async Task<IActionResult> DeleteMoto(Guid id) // recebe o id da moto
        {
            var moto = await _context.Motos.FindAsync(id); // busca a moto pelo id
            if (moto == null) return NotFound();

            // Verifica se a moto tem locações
            if (moto.TemLocacao) // pode ser substituído por um relacionamento de locações
            {
                return BadRequest("Não é possível remover a moto, pois ela possui registro de locação.");
            }

            _context.Motos.Remove(moto); // remove a moto
            await _context.SaveChangesAsync(); // salva as alterações

            return NoContent(); // retorna sucesso 204 no content
        }

    }
}