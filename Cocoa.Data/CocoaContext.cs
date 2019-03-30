using Cocoa.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cocoa.Data
{
    public class CocoaContext : DbContext
    {

        public DbSet<Sequence> Sequences { get; set; }
        public DbSet<SequencePose> SequencePoses { get; set; }
        public DbSet<Pose> Poses { get; set; }
        public DbSet<Joint> Joints { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=cocoa.db");
        }

    }
}
