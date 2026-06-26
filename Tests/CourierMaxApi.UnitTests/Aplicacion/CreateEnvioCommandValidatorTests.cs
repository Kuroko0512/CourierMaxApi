using Aplicacion.Envio.Create;
using FluentValidation.TestHelper;
using Xunit;

namespace CourierMaxApi.UnitTests.Aplicacion;

public class CreateEnvioCommandValidatorTests
{
    private readonly CreateEnvioCommandValidator _validator = new();

    private static CreateEnvioCommand ComandoValido() => new(
        TipoServicioId: 1,
        RemitenteNombre: "Ana Pérez",
        RemitenteTelefono: "3001234567",
        RemitenteDireccionRecogida: "Calle 1 #2-3",
        DestinatarioNombre: "Luis Gómez",
        DestinatarioTelefono: "3007654321",
        DestinatarioDireccionEntrega: "Calle 4 #5-6",
        PesoKg: 2.5m,
        LargoCm: 30m,
        AnchoCm: 20m,
        AltoCm: 10m,
        TipoPaqueteId: 1,
        CiudadOrigenId: 1,
        CiudadDestinoId: 2);

    [Fact]
    public void ComandoValido_NoDeberiaTenerErrores()
    {
        var resultado = _validator.TestValidate(ComandoValido());

        resultado.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void CiudadOrigenIgualADestino_DeberiaFallar()
    {
        var comando = ComandoValido() with { CiudadDestinoId = 1, CiudadOrigenId = 1 };

        var resultado = _validator.TestValidate(comando);

        resultado.ShouldHaveValidationErrorFor(x => x.CiudadDestinoId);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("9001234567")]
    public void TelefonoRemitenteInvalido_DeberiaFallar(string telefono)
    {
        var comando = ComandoValido() with { RemitenteTelefono = telefono };

        var resultado = _validator.TestValidate(comando);

        resultado.ShouldHaveValidationErrorFor(x => x.RemitenteTelefono);
    }

    [Fact]
    public void PesoFueraDeRango_DeberiaFallar()
    {
        var comando = ComandoValido() with { PesoKg = 0m };

        var resultado = _validator.TestValidate(comando);

        resultado.ShouldHaveValidationErrorFor(x => x.PesoKg);
    }
}
