using Dominio.V1.Vehiculo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arquitectura.Persistencia.Configuration
{
    public class VehiculoConfiguration : IEntityTypeConfiguration<VehiculoD>
    {
        public void Configure(EntityTypeBuilder<VehiculoD> builder)
        {
            builder.ToTable("tbl_Vehiculos");

            builder.HasKey(v => v.Id);
            builder.Property(v => v.Id).ValueGeneratedOnAdd();

            builder.Property(v => v.Placa).HasColumnName("Placa").HasMaxLength(10);
            builder.Property(v => v.CapacidadPesoKg).HasColumnName("CapacidadPesoKg").HasColumnType("decimal(8,2)");
            builder.Property(v => v.CapacidadVolumenM3).HasColumnName("CapacidadVolumenM3").HasColumnType("decimal(8,2)");
        }
    }
}
