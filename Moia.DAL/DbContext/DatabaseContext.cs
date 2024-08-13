using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Moia.Shared.Helpers;
using Moia.Shared.Models;
using Moia.Shared.ViewModels.DTOs;
using System.Security.Claims;

namespace Moia.DAL.databaseContext
{
    public class DatabaseContext : DbContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public DatabaseContext(DbContextOptions<DatabaseContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public override int SaveChanges()
        {
            int? UserId = null;
            var claim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var _UserId = EncryptionHelper.DecryptString(claim.Value);
                UserId = int.Parse(_UserId);
            }


            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                var createdOnProperty = entity.GetType().GetProperty("CreatedOn");
                var createdByProperty = entity.GetType().GetProperty("CreatedBy");
                if (createdOnProperty != null && createdByProperty != null && entry.State == EntityState.Added)
                {
                    createdByProperty.SetValue(entity, UserId);
                    createdOnProperty.SetValue(entity, DateTime.Now);
                }
                var modifiedOnProperty = entity.GetType().GetProperty("UpdatedOn");
                var modifiedByProperty = entity.GetType().GetProperty("UpdatedBy");
                if (modifiedOnProperty != null && modifiedByProperty != null && entry.State == EntityState.Modified)
                {
                    modifiedOnProperty.SetValue(entity, DateTime.Now);
                    modifiedByProperty.SetValue(entity, UserId);
                }
            }
            return base.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlServer("connectionstr");
            //}

            


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (IMutableForeignKey relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

        



            base.OnModelCreating(modelBuilder);
        }






        #region DbSets
        public virtual DbSet<MainUser> MainUsers { get; set; }
        public virtual DbSet<MainRole> MainRoles { get; set; }
        public virtual DbSet<EducationalLevel> EducationalLevels { get; set; }
        public virtual DbSet<Religion> Religions { get; set; } 
        public virtual DbSet<Muslime> Muslimes { get; set; }
        public virtual DbSet<Witness> Witness { get; set; }
        public virtual DbSet<Committee> Committees { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<IsslamRecognition> IsslamRecognition { get; set; } 
        public virtual DbSet<MinistryBransh> MinistryBranshs { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderHistory> OrderHistory { get; set; }
        public virtual DbSet<Exceptions> Exceptions { get; set; }
        public virtual DbSet<MainUserRole> MainUserRole { get; set; }
        public virtual DbSet<ResidenceIssuePlace> ResidenceIssuePlace { get; set; }
        public virtual DbSet<Preacher> Preachers { get; set; }
        public virtual DbSet<BranchNegoiationUsers> BranchNegoiationUsers { get; set; }
        public virtual DbSet<DepartmentNegoiationUsers> DepartmentNegoiationUsers { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<Localization> Localizations { get; set; }
        public virtual DbSet<OrderTransfere> OrderTransfere { get; set; }
        #endregion  
    }
}
