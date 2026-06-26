using Dominio.V1.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arquitectura.Persistencia.Configuration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<UsuarioD>
    {
        public void Configure(EntityTypeBuilder<UsuarioD> builder)
        {
            builder.ToTable("tbl_Usuarios");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            builder.Property(u => u.Nombre).HasColumnName("Nombre").HasMaxLength(120);
            builder.Property(u => u.RolId).HasColumnName("RolId");

            builder.HasOne(u => u.Rol).WithMany().HasForeignKey(u => u.RolId);
        }
    }
}
