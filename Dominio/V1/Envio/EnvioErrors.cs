using ErrorOr;

namespace Dominio.V1.Envio
{
    public static class EnvioErrors
    {
        public static Error NoEncontrado =>
            Error.NotFound("Envio.NoEncontrado", "El envío no existe.");

        public static Error TransicionInvalida(EstadoEnvio actual, EstadoEnvio nuevo) =>
            Error.Validation("Envio.TransicionInvalida", $"No se puede pasar de {actual} a {nuevo}.");

        public static Error MotivoRequerido =>
            Error.Validation("Envio.MotivoRequerido", "La cancelación requiere un motivo de al menos 5 caracteres.");

        public static Error ConductorRequerido =>
            Error.Validation("Envio.ConductorRequerido", "Asignar un envío requiere un conductor.");

        public static Error TipoServicioInvalido =>
            Error.Validation("Envio.TipoServicioInvalido", "El tipo de servicio no existe.");

        public static Error TipoPaqueteInvalido =>
            Error.Validation("Envio.TipoPaqueteInvalido", "El tipo de paquete no existe.");

        public static Error RutaSinTarifa =>
            Error.Validation("Envio.RutaSinTarifa", "No hay tarifa de distancia configurada para esa ruta.");

        public static Error ParametroTarifaNoConfigurado =>
            Error.Validation("Envio.ParametroTarifaNoConfigurado", "No hay parámetros de tarifa configurados.");

        public static Error RangoFechasInvalido =>
            Error.Validation("Envio.RangoFechasInvalido", "La fecha 'desde' no puede ser mayor que 'hasta'.");

        public static Error SinVehiculosDisponibles =>
            Error.Conflict("Envio.SinVehiculosDisponibles", "No hay vehículos con capacidad disponible para este envío.");
    }
}
