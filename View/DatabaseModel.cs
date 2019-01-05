namespace SpeechSynthesis
{
    using SpeechSynthesis.model;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DatabaseModel : DbContext
    {
        public DatabaseModel()
            : base("name=DatabaseModel")
        { }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }

    }
}