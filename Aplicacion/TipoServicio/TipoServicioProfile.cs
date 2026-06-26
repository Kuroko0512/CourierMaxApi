using Aplicacion.TipoServicio.Common;
using AutoMapper;
using Dominio.V1.TipoServicio;

namespace Aplicacion.TipoServicio
{
    public class TipoServicioProfile : Profile
    {
        public TipoServicioProfile()
        {
            CreateMap<TipoServicioD, TipoServicioResponse>();
        }
    }
}
