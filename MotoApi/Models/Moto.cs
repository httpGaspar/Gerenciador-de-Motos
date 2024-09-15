namespace MotoApi.Models
{
    public class Moto
    {
        public Guid Id { get; set; } // Usar Guid para ID
        public int Ano { get; set; }
        public string? Modelo { get; set; }
        public string? Placa { get; set; }

        // Você pode adicionar um campo para verificar locações no futuro:
        public bool TemLocacao { get; set; } // campo que indica se a moto possui registro de locação
    }
}
