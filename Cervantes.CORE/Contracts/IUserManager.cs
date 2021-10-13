namespace Cervantes.CORE.Contracts
{
    public interface IUserManager : IGenericManager<ApplicationUser>
    {
        ApplicationUser GetByUserId(string id);
    }
}
