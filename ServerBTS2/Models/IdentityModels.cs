﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;

namespace ServerBTS2.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<BaoCao> BaoCaos { get; set; }
        public DbSet<HinhAnhTram> HinhAnhTrams { get; set; }
        public DbSet<LichSuQuanLy> LichSuQuanLies { get; set; }
        public DbSet<MatDien> MatDiens { get; set; }
        public DbSet<NhaMang> NhaMangs { get; set; }
        public DbSet<NhatKy> NhatKies { get; set; }
        public DbSet<NhaTram> NhaTrams { get; set; }
        public DbSet<Tram> Trams { get; set; }
        public DbSet<UserBTS> UserBTSs { get; set; }
        public DbSet<TheLoaiAnh> TheLoaiAnhs { get; set; }
        public DbSet<HinhAnh> HinhAnhs { get; set; }
    }
}