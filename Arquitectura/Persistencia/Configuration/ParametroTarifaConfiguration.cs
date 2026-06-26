using Dominio.V1.ParametroTarifa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arquitectura.Persistencia.Configuration
{
    public class ParametroTarifaConfiguration : IEntityTypeConfiguration<ParametroTarifaD>
    {
        public void Configure(EntityTypeBuilder<ParametroTarifaD> builder)
        {
            builder.ToTable("tbl_ParametroTarifa");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.PesoBaseKg).HasColumnName("PesoBaseKg").HasColumnType("decimal(6,2)");
            builder.Property(p => p.RecargoPorKgAdicional).HasColumnName("RecargoPorKgAdicional").HasColumnType("decimal(12,2)");
        }
    }
}
