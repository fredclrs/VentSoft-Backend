namespace Domain.Dtos
{
    public class ArticuloCaracteristicaDto
    {
        public int Id { get; set; }
        public int IdCaracteristica { get; set; }

        /// <summary>Nombre de la característica, solo de lectura (para no pedir otro roundtrip).</summary>
        public string? NombreCaracteristica { get; set; }

        public string Valor { get; set; } = null!;
    }
}
