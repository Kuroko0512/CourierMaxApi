using Dominio.V1.Envio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arquitectura.Persistencia.Configuration
{
    public class HistorialEstadoConfiguration : IEntityTypeConfiguration<HistorialEstadoD>
    {
        public void Configure(EntityTypeBuilder<HistorialEstadoD> builder)
        {
            builder.ToTable("tbl_HistorialEstados");

            builder.HasKey(h => h.Id);
            builder.Property(h => h.Id).ValueGeneratedOnAdd();

            builder.Property(h => h.EnvioId).HasColumnName("EnvioId");
            builder.Property(h => h.EstadoAnterior).HasColumnName("EstadoAnterior").HasConversion<string>().HasMaxLength(15);
            builder.Property(h => h.EstadoNuevo).HasColumnName("EstadoNuevo").HasConversion<string>().HasMaxLength(15);
            builder.Property(h => h.FechaCambio).HasColumnName("FechaCambio");
            builder.Property(h => h.Motivo).HasColumnName("Motivo").HasMaxLength(250);
            builder.Property(h => h.RealizadoPorId).HasColumnName("RealizadoPorId");
        }
    }
}
