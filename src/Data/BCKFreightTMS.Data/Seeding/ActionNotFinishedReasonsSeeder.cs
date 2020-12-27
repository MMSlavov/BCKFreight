namespace BCKFreightTMS.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common;
    using BCKFreightTMS.Data.Models;

    public class ActionNotFinishedReasonsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ActionNotFinishedReasons.Any())
            {
                return;
            }

            await dbContext.ActionNotFinishedReasons.AddAsync(new ActionNotFinishedReason { Name = ActionNotFinishedReasons.NotArr });
            await dbContext.ActionNotFinishedReasons.AddAsync(new ActionNotFinishedReason { Name = ActionNotFinishedReasons.AtAdr1 });
            await dbContext.ActionNotFinishedReasons.AddAsync(new ActionNotFinishedReason { Name = ActionNotFinishedReasons.AtAdr2 });
            await dbContext.ActionNotFinishedReasons.AddAsync(new ActionNotFinishedReason { Name = ActionNotFinishedReasons.AtAdr3 });
            await dbContext.ActionNotFinishedReasons.AddAsync(new ActionNotFinishedReason { Name = ActionNotFinishedReasons.AtAdr4 });
            await dbContext.ActionNotFinishedReasons.AddAsync(new ActionNotFinishedReason { Name = ActionNotFinishedReasons.AtAdrToday });
            await dbContext.ActionNotFinishedReasons.AddAsync(new ActionNotFinishedReason { Name = ActionNotFinishedReasons.OffTime });
            await dbContext.ActionNotFinishedReasons.AddAsync(new ActionNotFinishedReason { Name = ActionNotFinishedReasons.TrPolice });
            await dbContext.ActionNotFinishedReasons.AddAsync(new ActionNotFinishedReason { Name = ActionNotFinishedReasons.Police });
            await dbContext.ActionNotFinishedReasons.AddAsync(new ActionNotFinishedReason { Name = ActionNotFinishedReasons.AccNoPart });
            await dbContext.ActionNotFinishedReasons.AddAsync(new ActionNotFinishedReason { Name = ActionNotFinishedReasons.AccPart });

            await dbContext.SaveChangesAsync();
        }
    }
}
