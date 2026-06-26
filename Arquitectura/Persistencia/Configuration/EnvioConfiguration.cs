using Dominio.V1.Envio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arquitectura.Persistencia.Configuration
{
    public class EnvioConfiguration : IEntityTypeConfiguration<EnvioD>
    {
        public void Configure(EntityTypeBuilder<EnvioD> builder)
        {
            builder.ToTable("tbl_Envios");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.CodigoRastreo).HasColumnName("CodigoRastreo").HasMaxLength(11);
            builder.HasIndex(e => e.CodigoRastreo).IsUnique();

            builder.Property(e => e.Estado).HasColumnName("Estado").HasConversion<string>().HasMaxLength(15);
            builder.Property(e => e.TipoServicioId).HasColumnName("TipoServicioId");

            builder.Property(e => e.CiudadOrigenId).HasColumnName("CiudadOrigenId");
            builder.Property(e => e.CiudadDestinoId).HasColumnName("CiudadDestinoId");
            builder.Property(e => e.ConductorId).HasColumnName("ConductorId");
            builder.Property(e => e.CostoTotal).HasColumnName("CostoTotal").HasColumnType("decimal(12,2)");

            builder.Property(e => e.FechaCreacion).HasColumnName("FechaCreacion");
            builder.Property(e => e.FechaAsignacion).HasColumnName("FechaAsignacion");
            builder.Property(e => e.FechaEntrega).HasColumnName("FechaEntrega");

            builder.OwnsOne(e => e.Remitente, r =>
            {
                r.Property(x => x.Nombre).HasColumnName("RemitenteNombre").HasMaxLength(120);
                r.Property(x => x.Telefono).HasColumnName("RemitenteTelefono").HasMaxLength(10);
                r.Property(x => x.DireccionRecogida).HasColumnName("RemitenteDireccionRecogida").HasMaxLength(250);
            });

            builder.OwnsOne(e => e.Destinatario, d =>
            {
                d.Property(x => x.Nombre).HasColumnName("DestinatarioNombre").HasMaxLength(120);
                d.Property(x => x.Telefono).HasColumnName("DestinatarioTelefono").HasMaxLength(10);
                d.Property(x => x.DireccionEntrega).HasColumnName("DestinatarioDireccionEntrega").HasMaxLength(250);
            });

            builder.OwnsOne(e => e.Paquete, p =>
            {
                p.Property(x => x.PesoKg).HasColumnName("PesoKg").HasColumnType("decimal(6,2)");
                p.Property(x => x.LargoCm).HasColumnName("LargoCm").HasColumnType("decimal(6,2)");
                p.Property(x => x.AnchoCm).HasColumnName("AnchoCm").HasColumnType("decimal(6,2)");
                p.Property(x => x.AltoCm).HasColumnName("AltoCm").HasColumnType("decimal(6,2)");
                p.Property(x => x.TipoPaqueteId).HasColumnName("TipoPaqueteId");

                p.Property(x => x.VolumenM3)
                    .HasColumnName("VolumenM3")
                    .HasColumnType("decimal(18,6)")
                    .HasComputedColumnSql("[LargoCm]*[AnchoCm]*[AltoCm]/1000000.0", stored: true)
                    .ValueGeneratedOnAddOrUpdate();
            });

            builder.HasMany(e => e.Historial)
                .WithOne()
                .HasForeignKey(h => h.EnvioId);
        }
    }
}
