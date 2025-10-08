﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;
internal class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "8D04DCE2-969A-4F1D-A0D5-9A1C8C0E8B33",
                Name = "Admin",
                NormalizedName = "ADMIN",
            },
            new IdentityRole
            {
                Id = "FAD04DCE2-969A-4F1D-A0D5-9A1C8C0E8B99",
                Name = "Manager",
                NormalizedName = "MANAGER",
            });

    }
}
