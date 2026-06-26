using Dominio.V1.TipoServicio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arquitectura.Persistencia.Configuration
{
    public class TipoServicioConfiguration : IEntityTypeConfiguration<TipoServicioD>
    {
        public void Configure(EntityTypeBuilder<TipoServicioD> builder)
        {
            builder.ToTable("tbl_TipoServicio");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();

            builder.Property(t => t.Codigo).HasColumnName("Codigo").HasMaxLength(30);
            builder.HasIndex(t => t.Codigo).IsUnique();

            builder.Property(t => t.Descripcion).HasColumnName("Descripcion").HasMaxLength(120);
            builder.Property(t => t.TarifaBase).HasColumnName("TarifaBase").HasColumnType("decimal(12,2)");
            builder.Property(t => t.DiasSla).HasColumnName("DiasSla");
            builder.Property(t => t.Orden).HasColumnName("Orden");
            builder.Property(t => t.Activo).HasColumnName("Activo");
        }
    }
}
