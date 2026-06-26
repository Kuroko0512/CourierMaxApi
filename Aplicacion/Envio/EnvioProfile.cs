using Aplicacion.Envio.Common;
using AutoMapper;
using Dominio.V1.Envio;
using Dominio.V1.Envio.ValueObjects;

namespace Aplicacion.Envio
{
    public class EnvioProfile : Profile
    {
        public EnvioProfile()
        {
            CreateMap<EnvioD, EnvioResponse>();
            CreateMap<Remitente, RemitenteResponse>();
            CreateMap<Destinatario, DestinatarioResponse>();
            CreateMap<Paquete, PaqueteResponse>();
            CreateMap<HistorialEstadoD, HistorialEstadoResponse>();
        }
    }
}
