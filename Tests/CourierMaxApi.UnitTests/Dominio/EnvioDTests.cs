using Dominio.V1.Envio;
using Dominio.V1.Envio.ValueObjects;
using FluentAssertions;
using Xunit;

namespace CourierMaxApi.UnitTests.Dominio;

public class EnvioDTests
{
    private static EnvioD CrearEnvioValido() => EnvioD.Crear(
        tipoServicioId: 1,
        remitente: new Remitente("Ana Pérez", "3001234567", "Calle 1 #2-3"),
        destinatario: new Destinatario("Luis Gómez", "3007654321", "Calle 4 #5-6"),
        paquete: new Paquete(2.5m, 30m, 20m, 10m, 1),
        ciudadOrigenId: 1,
        ciudadDestinoId: 2);

    [Fact]
    public void Crear_DeberiaIniciarEnEstadoCreadoConHistorialYEvento()
    {
        var envio = CrearEnvioValido();

        envio.Estado.Should().Be(EstadoEnvio.CREADO);
        envio.CodigoRastreo.Should().StartWith("CM-");
        envio.Historial.Should().ContainSingle()
            .Which.EstadoNuevo.Should().Be(EstadoEnvio.CREADO);
        envio.GetDomainEvents().Should().ContainSingle();
    }

    [Fact]
    public void CambiarEstado_DeCreadoAAsignado_ConConductor_DeberiaTenerExito()
    {
        var envio = CrearEnvioValido();

        var resultado = envio.CambiarEstado(EstadoEnvio.ASIGNADO, motivo: null, realizadoPorId: 1, conductorId: 99);

        resultado.IsError.Should().BeFalse();
        envio.Estado.Should().Be(EstadoEnvio.ASIGNADO);
        envio.ConductorId.Should().Be(99);
        envio.FechaAsignacion.Should().NotBeNull();
    }

    [Fact]
    public void CambiarEstado_AAsignadoSinConductor_DeberiaFallar()
    {
        var envio = CrearEnvioValido();

        var resultado = envio.CambiarEstado(EstadoEnvio.ASIGNADO, motivo: null, realizadoPorId: 1, conductorId: null);

        resultado.IsError.Should().BeTrue();
        resultado.FirstError.Should().Be(EnvioErrors.ConductorRequerido);
        envio.Estado.Should().Be(EstadoEnvio.CREADO);
    }

    [Fact]
    public void CambiarEstado_TransicionNoPermitida_DeberiaFallar()
    {
        var envio = CrearEnvioValido();

        var resultado = envio.CambiarEstado(EstadoEnvio.ENTREGADO, motivo: null, realizadoPorId: 1);

        resultado.IsError.Should().BeTrue();
        resultado.FirstError.Code.Should().Be("Envio.TransicionInvalida");
    }

    [Fact]
    public void CambiarEstado_CancelarSinMotivo_DeberiaFallar()
    {
        var envio = CrearEnvioValido();

        var resultado = envio.CambiarEstado(EstadoEnvio.CANCELADO, motivo: "x", realizadoPorId: 1);

        resultado.IsError.Should().BeTrue();
        resultado.FirstError.Should().Be(EnvioErrors.MotivoRequerido);
    }

    [Fact]
    public void CambiarEstado_EntregadoCompleto_DeberiaRegistrarFechaEntrega()
    {
        var envio = CrearEnvioValido();
        envio.CambiarEstado(EstadoEnvio.ASIGNADO, null, 1, conductorId: 5);
        envio.CambiarEstado(EstadoEnvio.EN_TRANSITO, null, 1);

        var resultado = envio.CambiarEstado(EstadoEnvio.ENTREGADO, null, 1);

        resultado.IsError.Should().BeFalse();
        envio.Estado.Should().Be(EstadoEnvio.ENTREGADO);
        envio.FechaEntrega.Should().NotBeNull();
    }

    [Fact]
    public void CalcularCosto_DeberiaAplicarRecargoPesoYPorcentaje()
    {
        var envio = CrearEnvioValido();

        envio.CalcularCosto(
            tarifaBase: 10000m,
            tarifaDistancia: 5000m,
            recargoPorcentajePaquete: 10m,
            pesoBaseKg: 1m,
            recargoPorKgAdicional: 2000m);

        envio.CostoTotal.Should().Be(19800m);
    }

    [Theory]
    [InlineData(3, 5, true)]
    [InlineData(3, 3, false)]
    [InlineData(5, 2, false)]
    public void EstaAtrasado_DeberiaEvaluarSlaCuandoNoEstaEntregadoNiCancelado(int sla, int transcurridos, bool esperado)
    {
        var envio = CrearEnvioValido();

        envio.EstaAtrasado(sla, transcurridos).Should().Be(esperado);
    }
}
