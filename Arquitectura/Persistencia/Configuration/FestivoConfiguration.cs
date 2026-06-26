using Dominio.V1.Festivo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arquitectura.Persistencia.Configuration
{
    public class FestivoConfiguration : IEntityTypeConfiguration<FestivoD>
    {
        public void Configure(EntityTypeBuilder<FestivoD> builder)
        {
            builder.ToTable("tbl_Festivos");

            builder.HasKey(f => f.Fecha);
            builder.Property(f => f.Fecha).HasColumnName("Fecha");
            builder.Property(f => f.Descripcion).HasColumnName("Descripcion").HasMaxLength(120);
        }
    }
}
