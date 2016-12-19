using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NCIProjects.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public int StudentNumber { get; set; }
        public virtual Student Students { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class Student
    {
        public int StudentID { get; set; }
        public int StudentNumber { get; set; }
        public String fname { get; set; }
        public String lname { get; set; }

        public int CourseID { get; set; }
        public int StreamID { get; set; }
        public int SupervisorID { get; set; }

        public virtual Course Course { get; set; }
        public virtual Stream Stream { get; set; }
        public virtual Supervisor Supervisor { get; set; }
        public virtual ICollection<Submission> Submission { get; set; }
        public virtual ICollection<File> Files { get; set; }

        //concatenate name and ID to display in SelectList in Submssion Creation
        public string StudentDetails { get { return StudentID + " " + fname + " " + lname; } }
    }

    public class Course
    {
        public int ID { get; set; }
        public String course_name { get; set; }

        public virtual ICollection<Student> Student { get; set; }
    }

    public class Stream
    {
        public int ID { get; set; }
        public String stream_name { get; set; }

        public virtual ICollection<Student> Student { get; set; }
    }

    public class Supervisor
    {
        public int ID { get; set; }
        public String first_name { get; set; }
        public String last_name { get; set; }
        public String email { get; set; }

        public virtual ICollection<Student> Student { get; set; }

        //concatenate name to display in SelectList
        public string SupervisorDetails { get { return first_name + " " + last_name; } }
    }

    public class Submission
    {
        public int ID { get; set; }
        public int StudentID { get; set; }
        public String linkedin_url { get; set; }
        public String project_title { get; set; }

        [DataType(DataType.MultilineText)]
        public String short_desc { get; set; }
   
        [DataType(DataType.MultilineText)]
        public String long_desc { get; set; }
      //public int StudentTechnologiesID { get; set; }

        public virtual Student Student { get; set; }
      //public virtual StudentTechnologies StudentTechnologies { get; set; }
    }

   /* public class StudentTechnologies
    {
        public int ID { get; set; }
        public int TechnologiesID { get; set; }

        public virtual Technologies Technologies { get; set; }
        public virtual ICollection<Submission> Submissions { get; set; }
    }

    public class Technologies
    {
        public int ID { get; set; }
        public String tech_name { get; set; }

        public virtual ICollection<StudentTechnologies> StudentTechnologies { get; set; }
    } */

    public enum FileType
    {
        Avatar = 1, Photo
    }

    public class File
    {
        public int FileId { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public FileType FileType { get; set; }
        public int StudentID { get; set; }
        public virtual Student Student { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual IDbSet<Student> Students { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>().ToTable("UserAccount").Property(p => p.Id).HasColumnName("UserAccountId");
            modelBuilder.Entity<ApplicationUser>().ToTable("UserAccount").Property(p => p.Id).HasColumnName("UserAccountId");

            modelBuilder.Entity<IdentityRole>().ToTable("Role");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin");

            modelBuilder.Entity<IdentityUser>().Ignore(c => c.PhoneNumber)
                                               .Ignore(c => c.PhoneNumberConfirmed)
                                               .Ignore(c => c.LockoutEnabled)
                                               .Ignore(c => c.LockoutEndDateUtc)
                                               .Ignore(c => c.AccessFailedCount)
                                               .Ignore(c => c.TwoFactorEnabled);
        }

        public DbSet<File> Files { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Stream> Streams { get; set; }

        public DbSet<Supervisor> Supervisors { get; set; }

        public DbSet<Submission> Submissions { get; set; }

      //public DbSet<StudentTechnologies> StudentTechnologies { get; set; }
    }
}