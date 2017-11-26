using Microsoft.EntityFrameworkCore;

namespace Subjector.Data.Entities
{
    public partial class SubjectorContext : DbContext
    {
        public virtual DbSet<Activity> Activity { get; set; }
        public virtual DbSet<ActivityType> ActivityType { get; set; }
        public virtual DbSet<Cert> Cert { get; set; }
        public virtual DbSet<Result> Result { get; set; }
        public virtual DbSet<Session> Session { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<SubjectActivity> SubjectActivity { get; set; }
        public virtual DbSet<User> User { get; set; }

        public SubjectorContext(DbContextOptions<SubjectorContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>(entity =>
            {
                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Activity)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Activity_User");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Activity)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Activity_ActivityType");
            });

            modelBuilder.Entity<ActivityType>(entity =>
            {
                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ActivityType)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActivityType_User");
            });

            modelBuilder.Entity<Cert>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CertNumber)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Cert)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cert_User");
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.SubjectActivityId });

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.GradedByNavigation)
                    .WithMany(p => p.ResultGradedByNavigation)
                    .HasForeignKey(d => d.GradedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Result_User");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.ResultStudent)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Result_Student");

                entity.HasOne(d => d.SubjectActivity)
                    .WithMany(p => p.Result)
                    .HasPrincipalKey(p => p.Id)
                    .HasForeignKey(d => d.SubjectActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Result_SubjectActivity");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.LastAction).HasColumnType("datetime");

                entity.Property(e => e.Token).HasMaxLength(250);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Session)
                    .HasForeignKey<Session>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Session_User");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Subject)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Subject_User");
            });

            modelBuilder.Entity<SubjectActivity>(entity =>
            {
                entity.HasKey(e => new { e.SubjectId, e.ActivityId });

                entity.HasIndex(e => e.Id)
                    .HasName("UQ__SubjectA__3214EC0659853781")
                    .IsUnique();

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.SubjectActivity)
                    .HasForeignKey(d => d.ActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubjectActivity_Activity");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.SubjectActivity)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubjectActivity_User");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.SubjectActivity)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubjectActivity_Subject");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Password);

                entity.Property(e => e.RefCode).HasMaxLength(50);
            });
        }
    }
}
