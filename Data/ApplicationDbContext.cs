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

                    i => i.HasOne<Pizza>().WithMany()
                        .HasForeignKey("PizzaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PizzaIngredient_Pizza"),

                    pi =>
                    {
                        pi.HasKey("PizzaId", "IngredientId");
                        pi.ToTable("PizzaIngredient");
                    });
              
            });


            modelBuilder.Entity<Client>()
                .HasMany(cl => cl.Commandes)
                .WithOne(co => co.Client)
                .HasForeignKey(cl => cl.ClientID)
                .IsRequired();


            modelBuilder.Entity<Commande>()
               .HasMany(c => c.ligneDeCommandes)
               .WithOne(lc => lc.Commande)
               .HasForeignKey(lc => lc.CommandeId)
               .IsRequired();


            modelBuilder.Entity<LigneDeCommande>()
                .HasOne(lc => lc.Pizza)
                .WithOne(p => p.LigneDeCommande)?
                .HasForeignKey<LigneDeCommande>(lc => lc.PizzaId)
                .IsRequired();

          
        }

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Commande> Commandes { get; set;}
        public DbSet<LigneDeCommande> LigneDeCommandes { get; set; }
    }
}
