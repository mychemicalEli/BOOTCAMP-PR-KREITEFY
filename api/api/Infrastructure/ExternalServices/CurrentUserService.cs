using System.Security.Claims;
using api.Application.Dtos;
using api.Application.Services.Interfaces;
using api.Domain.Persistence;

namespace api.Infrastructure.ExternalServices
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<UserDtoResponse> GetUserFromTokenAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
                return null;

            var user = await _userRepository.GetByIdAsync(userId);
            return user == null
                ? null
                : new UserDtoResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password,
                    RoleId = user.RoleId,
                    RoleName = user.Role.Name
                };
        }
    }
}