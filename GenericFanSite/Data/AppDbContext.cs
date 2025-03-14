﻿using GenericFanSite.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GenericFanSite.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        // constructor just calls the base class constructor
        public AppDbContext(
           DbContextOptions<AppDbContext> options) : base(options) { }
        // one DbSet for each domain model class
        public DbSet<ForumPost> ForumPosts { get; set; }
    }
}
