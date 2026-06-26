namespace Dominio.V1.Festivo
{
    public class FestivoD
    {
        public DateOnly Fecha { get; private set; }

        public string? Descripcion { get; private set; }

        private FestivoD() { }
    }
}
