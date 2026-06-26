using Aplicacion.Rol.Common;
using AutoMapper;
using Dominio.V1.Rol;

namespace Aplicacion.Rol
{
    public class RolProfile : Profile
    {
        public RolProfile()
        {
            CreateMap<RolD, RolResponse>();
        }
    }
}
