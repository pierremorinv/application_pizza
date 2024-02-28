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
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_PizzaIngredient_Ingredient"),

                    i => i.HasOne<Pizza>().WithMany()
                        .HasForeignKey("PizzaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_PizzaIngredient_Pizza"),

                    pi =>
                    {
                        pi.HasKey("PizzaId", "IngredientId");
                        pi.ToTable("PizzaIngredient");
                    });

            });

            modelBuilder.Entity<LigneDeCommande>(entity =>
            {
                entity.HasMany(o => o.Ingredients).WithMany(i => i.LigneDeCommandes)
                    .UsingEntity<Dictionary<string, object>>(
                    "LigneDeCommandeIngredient",
                    oi => oi.HasOne<Ingredient>().WithMany()
                    .HasForeignKey("IngredientId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Fk_LigneDeCommandeIngredient_Ingredient"),

                    i => i.HasOne<LigneDeCommande>().WithMany()
                    .HasForeignKey("LigneDeCommandeId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_LigneDeCommandeIngredient_LigneDeCommande"),

                    oi =>
                    {
                        oi.HasKey("LigneDeCommandeId", "IngredientId");
                        oi.ToTable("LigneDeCommandeIngredient");
                    }
                    );
            });

            modelBuilder.Entity<Compte>()
                 .HasOne(co => co.Client)
                 .WithOne(cl => cl.Compte)
                 .HasForeignKey<Client>(co => co.CompteId)
                 .IsRequired();

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

            modelBuilder.Entity<Pizza>()
                .HasMany(p => p.LigneDeCommandes)
                .WithOne(lc => lc.Pizza)
                .HasForeignKey(lc => lc.PizzaId)
                .IsRequired();
        }

        public DbSet<Compte> Comptes { get; set; }
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<LigneDeCommande> LigneDeCommandes { get; set; }



    }
}
