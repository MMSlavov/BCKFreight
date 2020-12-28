namespace BCKFreightTMS.Services.Data.Tests
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data;
    using BCKFreightTMS.Data.Common.Models;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Data.Repositories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Moq;

    public class RepositoryFactory
    {
        public EfDeletableEntityRepository<T> GetEfDeletableEntityRepository<T>(ApplicationDbContext dbContext, string adminId = null, bool isUserAdmin = false)
                where T : class, IDeletableEntity, ICompanyEntity
        {
            var httpConAcc = new Mock<IHttpContextAccessor>();
            httpConAcc.Setup(x => x.HttpContext.User.IsInRole(RoleNames.Admin.ToString()))
                    .Returns(isUserAdmin);
            httpConAcc.Setup(x => x.HttpContext.User)
                    .Returns(new ClaimsPrincipal());

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userMan = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userMan.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new ApplicationUser { AdminId = adminId }));

            return new EfDeletableEntityRepository<T>(dbContext, httpConAcc.Object, userMan.Object);
        }
    }
}
