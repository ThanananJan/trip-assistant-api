using JWTAuthentication.Library.Model.Dto;

namespace JWTAuthentication.Library.Interfaces
{
    public interface IUserService
    {
        public void CreateUser(UserRequest request);
        public void UpdateUser(UserRequest request);
    }
}
