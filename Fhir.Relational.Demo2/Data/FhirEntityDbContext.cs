// -------------------------------------------------------------------------------------------------
// Copyright (c) Integrated Health Information Systems Pte Ltd. All rights reserved.
// -------------------------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;

namespace Fhir.Relational.Demo2.Data
{
    public class FhirEntityDbContext : DbContext
    {
        public FhirEntityDbContext(DbContextOptions<FhirEntityDbContext> options)
            : base(options)
        {
#if DEBUG
            //Database.EnsureDeleted();
            Database.EnsureCreated();
#endif
        }

        public DbSet<AppointmentEntity> Appointment => Set<AppointmentEntity>();
        public DbSet<PatientEntity> Patient => Set<PatientEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseFhirConventions(this);
        }
    }
}
