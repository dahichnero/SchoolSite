using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SchkalkaB.Models;
using DayOfWeek = SchkalkaB.Models.DayOfWeek;

namespace SchkalkaB.Data;

public partial class SchkalkaDbContext : DbContext
{
    public SchkalkaDbContext()
    {
    }

    public SchkalkaDbContext(DbContextOptions<SchkalkaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<DayOfWeek> DayOfWeeks { get; set; }

    public virtual DbSet<Director> Directors { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Parent> Parents { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<StatusParent> StatusParents { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentParent> StudentParents { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<TeacherSubject> TeacherSubjects { get; set; }

    public virtual DbSet<TimeTable> TimeTables { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=Schkalka; Trusted_Connection=True; MultipleActiveResultSets=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.ToTable("Class");

            entity.Property(e => e.NameClass).HasMaxLength(50);

            entity.HasOne(d => d.ClassRukNavigation).WithMany(p => p.Classes)
                .HasForeignKey(d => d.ClassRuk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Class_Teacher");
        });

        modelBuilder.Entity<DayOfWeek>(entity =>
        {
            entity.HasKey(e => e.DayId);

            entity.ToTable("DayOfWeek");
        });

        modelBuilder.Entity<Director>(entity =>
        {
            entity.ToTable("Director");

            entity.Property(e => e.DirectorLastName).HasMaxLength(50);
            entity.Property(e => e.DirectorName).HasMaxLength(50);
            entity.Property(e => e.DirectorSurName).HasMaxLength(50);

            entity.HasOne(d => d.GenderNavigation).WithMany(p => p.Directors)
                .HasForeignKey(d => d.Gender)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Director_Gender");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Directors)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Director_Status");

            entity.HasOne(d => d.UserINavigation).WithMany(p => p.Directors)
                .HasForeignKey(d => d.UserI)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Director_User");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("Event");

            entity.HasOne(d => d.TeacherNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.Teacher)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Event_Teacher");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.ToTable("Gender");

            entity.Property(e => e.GenderName).HasMaxLength(200);
        });

        modelBuilder.Entity<Parent>(entity =>
        {
            entity.HasKey(e => e.ParentsId).HasName("PK_Parents");

            entity.ToTable("Parent");

            entity.Property(e => e.LastNameParent).HasMaxLength(50);
            entity.Property(e => e.NameParent).HasMaxLength(50);
            entity.Property(e => e.SurNameParent).HasMaxLength(50);

            entity.HasOne(d => d.StatusParentNavigation).WithMany(p => p.Parents)
                .HasForeignKey(d => d.StatusParent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Parent_StatusParents");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.NameRole).HasMaxLength(10);
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("Status");

            entity.Property(e => e.StatusName).HasMaxLength(100);
        });

        modelBuilder.Entity<StatusParent>(entity =>
        {
            entity.HasKey(e => e.StatusParentsId);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Student");

            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.SurName).HasMaxLength(50);

            entity.HasOne(d => d.ClassNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.Class)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_Class");

            entity.HasOne(d => d.GenderNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.Gender)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_Gender");

            entity.HasOne(d => d.UserINavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.UserI)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_User");
        });

        modelBuilder.Entity<StudentParent>(entity =>
        {
            entity.ToTable("StudentParent");

            entity.HasOne(d => d.ParentNavigation).WithMany(p => p.StudentParents)
                .HasForeignKey(d => d.Parent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentParent_Parent");

            entity.HasOne(d => d.StudentNavigation).WithMany(p => p.StudentParents)
                .HasForeignKey(d => d.Student)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentParent_Student");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK_Table_1");

            entity.ToTable("Subject");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.ToTable("Teacher");

            entity.Property(e => e.TeacherLastName).HasMaxLength(50);
            entity.Property(e => e.TeacherName).HasMaxLength(50);
            entity.Property(e => e.TeacherSurName).HasMaxLength(50);

            entity.HasOne(d => d.GenderNavigation).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.Gender)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Teacher_Gender");

            entity.HasOne(d => d.UserlNavigation).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.Userl)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Teacher_User");
        });

        modelBuilder.Entity<TeacherSubject>(entity =>
        {
            entity.ToTable("TeacherSubject");

            entity.HasOne(d => d.SubjectNavigation).WithMany(p => p.TeacherSubjects)
                .HasForeignKey(d => d.Subject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeacherSubject_Subject");

            entity.HasOne(d => d.TeacherNavigation).WithMany(p => p.TeacherSubjects)
                .HasForeignKey(d => d.Teacher)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeacherSubject_Teacher");
        });

        modelBuilder.Entity<TimeTable>(entity =>
        {
            entity.ToTable("TimeTable");

            entity.Property(e => e.Cabinet).HasMaxLength(50);

            entity.HasOne(d => d.ClassNavigation).WithMany(p => p.TimeTables)
                .HasForeignKey(d => d.Class)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TimeTable_Class");

            entity.HasOne(d => d.DayOfWeekNavigation).WithMany(p => p.TimeTables)
                .HasForeignKey(d => d.DayOfWeek)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TimeTable_DayOfWeek");

            entity.HasOne(d => d.TeacherSubjectNavigation).WithMany(p => p.TimeTables)
                .HasForeignKey(d => d.TeacherSubject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TimeTable_TeacherSubject");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Role)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
