using JWTAuthentication.Library.Interfaces;
using JWTAuthentication.Library.Model.DB;
using JWTAuthentication.Library.Model.Dto;
using JWTAuthentication.Library.Model.Utilities;

namespace JWTAuthentication.Library.Services
{
    internal class UserService(JwtAuthDbContext db) : IUserService
    {
        public void CreateUser(UserRequest request)
    {
        if (!ValidCreateUser(request))
        {
            throw new InvalidDataException(Utility.UserErrorHandler["duplicate"]);
        }
        var user = GetUser(request);
        db.Users.Add(user);
        db.SaveChanges();

    }

    public void UpdateUser(UserRequest request)
    {
        if (!ValidUpdateUser(request))
        {
            throw new InvalidDataException(Utility.UserErrorHandler["duplicate"]);

        }
        var user = db.Users.First(p => p.NamUser == request.UserName);
        user.IsActive = request.IsActive;
        db.SaveChanges();
    }

    private User GetUser(UserRequest request)
    {
        string identityProvider = Utility.IdentityProvider["aws"];
        return new User()
        {
            NamUser = request.UserName,
            IsActive = request.IsActive,
            DtmCreate = DateTime.UtcNow,
            DtmUpdate = DateTime.UtcNow,
            IdentityProviderName = identityProvider
        };
    }

    private bool ValidCreateUser(UserRequest request)
    {
        var existUser = db.Users.FirstOrDefault(p => p.NamUser == request.UserName);
        return existUser == null ? true : false;
    }
    private bool ValidUpdateUser(UserRequest request)
    {
        var existUser = db.Users.FirstOrDefault(p => p.NamUser == request.UserName);
        return existUser == null ? false : true;
    }
}
}
