using Dominio.V1.Conductor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arquitectura.Persistencia.Configuration
{
    public class ConductorConfiguration : IEntityTypeConfiguration<ConductorD>
    {
        public void Configure(EntityTypeBuilder<ConductorD> builder)
        {
            builder.ToTable("tbl_Conductores");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.Nombre).HasColumnName("Nombre").HasMaxLength(120);
            builder.Property(c => c.VehiculoId).HasColumnName("VehiculoId");
            builder.Property(c => c.Activo).HasColumnName("Activo");
        }
    }
}
