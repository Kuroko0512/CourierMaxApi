using Aplicacion.TipoPaquete.Common;
using AutoMapper;
using Dominio.V1.TipoPaquete;

namespace Aplicacion.TipoPaquete
{
    public class TipoPaqueteProfile : Profile
    {
        public TipoPaqueteProfile()
        {
            CreateMap<TipoPaqueteD, TipoPaqueteResponse>();
        }
    }
}
