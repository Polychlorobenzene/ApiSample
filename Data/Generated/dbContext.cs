
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

using ApiSample.Data.Entities;

namespace ApiSample.Data
{
    public partial class ApiSampleDbContext : DbContext
    {
		public ApiSampleDbContext() : base("EntityConnection") { }
        public ApiSampleDbContext(string connectionName) : base(connectionName) { }
        public DbSet<Person> Persons { get; set; }
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
			CustomOnModelCreating(modelBuilder);
			
			#region Base Tables
			modelBuilder
                .Entity<Person>()
                .ToTable("Person", "dbo")
                .HasKey(person => person.Id)

                .Property(person => person.Id)
                .HasColumnName("PersonId")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

			#endregion Base Tables

			#region Associations
			#endregion Associations
					
			OnModelCreatingWithIdentities(modelBuilder);
			base.OnModelCreating(modelBuilder);
		}

		protected virtual void OnModelCreatingWithIdentities(DbModelBuilder modelBuilder)
		{
				}
		partial void CustomOnModelCreating(DbModelBuilder modelBuilder);
	}

}

