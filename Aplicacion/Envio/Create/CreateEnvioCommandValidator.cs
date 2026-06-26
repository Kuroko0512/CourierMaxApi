using FluentValidation;

namespace Aplicacion.Envio.Create
{
    public class CreateEnvioCommandValidator : AbstractValidator<CreateEnvioCommand>
    {
        public CreateEnvioCommandValidator()
        {
            RuleFor(x => x.TipoServicioId).GreaterThan(0);
            RuleFor(x => x.TipoPaqueteId).GreaterThan(0);

            RuleFor(x => x.RemitenteTelefono).Matches(@"^[36]\d{9}$");
            RuleFor(x => x.DestinatarioTelefono).Matches(@"^[36]\d{9}$");

            RuleFor(x => x.RemitenteNombre).NotEmpty().MaximumLength(120);
            RuleFor(x => x.DestinatarioNombre).NotEmpty().MaximumLength(120);
            RuleFor(x => x.RemitenteDireccionRecogida).NotEmpty().MaximumLength(250);
            RuleFor(x => x.DestinatarioDireccionEntrega).NotEmpty().MaximumLength(250);

            RuleFor(x => x.PesoKg).InclusiveBetween(0.1m, 100m);
            RuleFor(x => x.LargoCm).InclusiveBetween(1m, 200m);
            RuleFor(x => x.AnchoCm).InclusiveBetween(1m, 200m);
            RuleFor(x => x.AltoCm).InclusiveBetween(1m, 200m);

            RuleFor(x => x.CiudadDestinoId)
                .NotEqual(x => x.CiudadOrigenId)
                .WithMessage("La ciudad de origen y destino no pueden ser la misma.");
        }
    }
}
