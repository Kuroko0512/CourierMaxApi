using Dominio.V1.TipoPaquete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arquitectura.Persistencia.Configuration
{
    public class TipoPaqueteConfiguration : IEntityTypeConfiguration<TipoPaqueteD>
    {
        public void Configure(EntityTypeBuilder<TipoPaqueteD> builder)
        {
            builder.ToTable("tbl_TipoPaquete");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();

            builder.Property(t => t.Codigo).HasColumnName("Codigo").HasMaxLength(30);
            builder.HasIndex(t => t.Codigo).IsUnique();

            builder.Property(t => t.Descripcion).HasColumnName("Descripcion").HasMaxLength(120);
            builder.Property(t => t.RecargoPorcentaje).HasColumnName("RecargoPorcentaje").HasColumnType("decimal(5,2)");
            builder.Property(t => t.Orden).HasColumnName("Orden");
            builder.Property(t => t.Activo).HasColumnName("Activo");
        }
    }
}
