using APP_KTRA_ROUTER.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace APP_KTRA_ROUTER.Database
{
   public class EMEC_DB_Context : DbContext
    {
        public DbSet<Information> Informations { get; set; }
        public DbSet<DonVi> Dvi_QUANLYS { get; set; }
        public DbSet<db_TRAM> Db_TRAMs { get; set; }
        public DbSet<Save_Path> Save_Paths { get; set; }
        private readonly string _databasePath;

        public EMEC_DB_Context(string databasePath)
        {
            _databasePath = databasePath;
            //Database.EnsureDeleted();             
            //Database.EnsureCreated();
           //Database.Migrate();

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(string.Format("Filename={0}", _databasePath));


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Information>().HasKey(p => new { p.SERY_CTO, p.LOAI_BCS, p.MA_GC });
            modelBuilder.Entity<db_TRAM>().HasKey(ba => new { ba.MA_DVIQLY, ba.MA_TRAM });
            modelBuilder.Entity<Save_Path >().HasKey(ba => new { ba.MA_TRAM, ba.Path  });
        }
    }
}
