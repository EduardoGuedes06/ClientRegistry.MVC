using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientRegistry.Domain.Models;

namespace ClientRegistry.Data.Mappings
{
    public class ClientMapping : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Type)
                .IsRequired()
                .HasColumnType("varchar(2)");

            builder.Property(p => p.Document)
                .IsRequired()
                .HasColumnType("varchar(18)");

            builder.Property(p => p.Phone)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(p => p.RegisterDateTime)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(p => p.Active)
                .IsRequired()
                .HasDefaultValue(true);

            builder.ToTable("Clients");
        }
    }
}
