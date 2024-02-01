using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pizza>(entity =>
            {
                entity.HasMany(p => p.Ingredients).WithMany(p => p.Pizzas)
                .UsingEntity<Dictionary<string, object>>(
                    "PizzaIngredient",
                    pi => pi.HasOne<Ingredient>().WithMany()
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PizzaIngredient_Ingredient"),
                    l => l.HasOne<Pizza>().WithMany()
                        .HasForeignKey("PizzaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PizzaIngredient_Pizza"),
                    j =>
                    {
                        j.HasKey("PizzaId", "IngredientId");
                        j.ToTable("PizzaIngredient");
                    });
              
            });
            modelBuilder.Entity<Client>()
                .HasMany(e => e.Commandes)
                .WithOne(e => e.Client)
                .HasForeignKey(e => e.ClientID)
                .IsRequired();
       
            modelBuilder.Entity<LigneDeCommande>()
                .HasOne(e => e.Pizza)
                .WithOne(e => e.LigneDeCommande)?
                .HasForeignKey<LigneDeCommande>(e => e.PizzaId)
                .IsRequired();
                
        }

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Commande> Commandes { get; set;}
        public DbSet<LigneDeCommande> LigneDeCommandes { get; set; }
    }
}
