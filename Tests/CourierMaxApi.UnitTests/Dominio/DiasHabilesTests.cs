using Dominio.Servicios;
using FluentAssertions;
using Xunit;

namespace CourierMaxApi.UnitTests.Dominio;

public class DiasHabilesTests
{
    private static readonly IReadOnlySet<DateOnly> SinFestivos = new HashSet<DateOnly>();

    [Fact]
    public void Transcurridos_DeberiaExcluirSabadosYDomingos()
    {
        var desde = new DateTime(2026, 6, 17);
        var hasta = new DateTime(2026, 6, 24);

        var dias = DiasHabiles.Transcurridos(desde, hasta, SinFestivos);

        dias.Should().Be(5);
    }

    [Fact]
    public void Transcurridos_DeberiaExcluirFestivos()
    {
        var desde = new DateTime(2024, 6, 3);
        var hasta = new DateTime(2024, 6, 7);
        var festivos = new HashSet<DateOnly> { new(2024, 6, 5) };

        var dias = DiasHabiles.Transcurridos(desde, hasta, festivos);

        dias.Should().Be(3);
    }

    [Fact]
    public void Transcurridos_MismoDia_DeberiaSerCero()
    {
        var dia = new DateTime(2024, 6, 3);

        DiasHabiles.Transcurridos(dia, dia, SinFestivos).Should().Be(0);
    }
}
