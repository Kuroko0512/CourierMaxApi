namespace Dominio.V1.Envio.ValueObjects
{
    public record Paquete(decimal PesoKg, decimal LargoCm, decimal AnchoCm, decimal AltoCm, int TipoPaqueteId)
    {
        public decimal VolumenM3 { get; init; } = LargoCm * AnchoCm * AltoCm / 1_000_000m;
    }
}
