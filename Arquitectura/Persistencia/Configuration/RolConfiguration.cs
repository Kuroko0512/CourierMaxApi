using Dominio.V1.Rol;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arquitectura.Persistencia.Configuration
{
    public class RolConfiguration : IEntityTypeConfiguration<RolD>
    {
        public void Configure(EntityTypeBuilder<RolD> builder)
        {
            builder.ToTable("tbl_Rol");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();

            builder.Property(t => t.Codigo).HasColumnName("Codigo").HasMaxLength(30);
            builder.HasIndex(t => t.Codigo).IsUnique();

            builder.Property(t => t.Descripcion).HasColumnName("Descripcion").HasMaxLength(120);
            builder.Property(t => t.Orden).HasColumnName("Orden");
            builder.Property(t => t.Activo).HasColumnName("Activo");
        }
    }
}
