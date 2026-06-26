using Dominio.V1.Usuario;
using ErrorOr;
using MediatR;

namespace Aplicacion.Auth.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<LoginResponse>>
    {
        private const string ClaveAcceso = "Courier123*";

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginCommandHandler(IUsuarioRepository usuarioRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _usuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
            _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
        }

        public async Task<ErrorOr<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.GetByNombreAsync(command.Usuario);
            if (usuario is null || command.Password != ClaveAcceso)
            {
                return AuthErrors.CredencialesInvalidas;
            }

            var rol = usuario.Rol?.Codigo ?? string.Empty;
            var token = _jwtTokenGenerator.Generate(usuario.Id, usuario.Nombre, rol);

            return new LoginResponse(usuario.Id, usuario.Nombre, rol, token);
        }
    }
}
