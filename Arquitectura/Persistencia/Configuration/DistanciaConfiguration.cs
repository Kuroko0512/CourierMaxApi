using Dominio.V1.Distancia;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arquitectura.Persistencia.Configuration
{
    public class DistanciaConfiguration : IEntityTypeConfiguration<DistanciaD>
    {
        public void Configure(EntityTypeBuilder<DistanciaD> builder)
        {
            builder.ToTable("tbl_Distancias");

            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id).ValueGeneratedOnAdd();

            builder.Property(d => d.CiudadOrigenId).HasColumnName("CiudadOrigenId");
            builder.Property(d => d.CiudadDestinoId).HasColumnName("CiudadDestinoId");
            builder.Property(d => d.DistanciaKm).HasColumnName("DistanciaKm");
            builder.Property(d => d.TarifaDistancia).HasColumnName("TarifaDistancia").HasColumnType("decimal(12,2)");
        }
    }
}
