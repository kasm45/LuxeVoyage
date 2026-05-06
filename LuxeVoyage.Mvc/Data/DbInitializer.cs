using LuxeVoyage.Mvc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Data;

public static class DbInitializer
{
    private const string AdminRole = "Admin";

    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        Directory.CreateDirectory(Path.Combine(env.ContentRootPath, "Data"));

        var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await ctx.Database.MigrateAsync();

        if (!await roleManager.RoleExistsAsync(AdminRole))
            await roleManager.CreateAsync(new IdentityRole(AdminRole));

        await SeedUsers(userManager);
        await SeedCatalog(ctx);
    }

    private static async Task SeedUsers(UserManager<ApplicationUser> users)
    {
        const string adminEmail = "admin@luxevoyage.com";
        if (await users.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                DisplayName = "Site Admin"
            };
            var r = await users.CreateAsync(admin, "Admin123!");
            if (r.Succeeded)
                await users.AddToRoleAsync(admin, AdminRole);
        }

        const string travelerEmail = "traveler@demo.com";
        if (await users.FindByEmailAsync(travelerEmail) == null)
        {
            var traveler = new ApplicationUser
            {
                UserName = travelerEmail,
                Email = travelerEmail,
                EmailConfirmed = true,
                DisplayName = "Demo Traveler"
            };
            await users.CreateAsync(traveler, "Traveler123!");
        }
    }

    private static async Task SeedCatalog(ApplicationDbContext ctx)
    {
        foreach (var seed in SeedDestinationsData.All)
        {
            if (!await ctx.Destinations.AnyAsync(d => d.Title == seed.Title))
                ctx.Destinations.Add(seed);
        }

        if (!await ctx.Experiences.AnyAsync())
            ctx.Experiences.AddRange(
            new Experience
            {
                Title = "The Alpine Sanctuary Peak",
                Slug = "alpine-sanctuary",
                Category = ExperienceCategoryKind.NatureEscapes,
                Region = RegionKind.Europe,
                ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuDpmw05ns8yzDkPxrYnRRlDPS_kLOmlM1E4n3jMq0svxE8mtVpDhx4KadtTBz2h08lrYc318-cja32qurNIFBN60frUV1IzO0oj3O06CpvA9D2965Ryfsd-2chVQW3B-tbKZUremaGddSkJ9wfz5OqAP6ly8S6ZvXz9xzOmm7ETg-HFvEgQjE8Z2Gj_HLoDzTMsEsZQpo-T83n95uQrfnFMQsATDwyKWDSCcz8RyRDy7BgctFww_e7ZwvJChBu_lSEhJLKGFNVwpG0",
                Summary = "Secluded peaks and sunrise vistas above the treeline.",
                LocationLabel = "Swiss Alps, Switzerland",
                Rating = 4.9,
                PriceHint = "$520",
                BreadcrumbRegion = "Europe",
                BreadcrumbCity = "Swiss Alps",
                BreadcrumbCurrent = "Alpine Sanctuary",
                TagLine = "Nature Escapes"
            },
            new Experience
            {
                Title = "The Parisian Gallery Tour",
                Slug = "parisian-gallery",
                Category = ExperienceCategoryKind.ArtCulture,
                Region = RegionKind.Europe,
                ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuCHnkC4jqzoSm6c-wP1_id_3vgsakis6jVASXJ569o81eqQHWQNYFR3iPVgz0IOTghlkyjf7j7FMJqO3VeU2eEK30Ov1vHp-eWi8kHVcmKwYhg6cr88dEdZ5539y_UJ6ab9MWrsmM3-pIzm6FyCT74rrTq2L1PF7CfbKA2vUb7_cFHgBd-y59p2EuuoQkD0W3E2vH-C6X1qfCcw6aVsewT79jVi30DWH7impJxUrzfnmfbmUDpv3qPfvAI-nFIBpTq-_T0XJTznNZQ",
                Summary = "Exclusive access to private collections and modern installations.",
                LocationLabel = "Paris, France",
                Rating = 4.95,
                PriceHint = "$680",
                BreadcrumbRegion = "Europe",
                BreadcrumbCity = "Paris",
                BreadcrumbCurrent = "Gallery Circuit",
                TagLine = "Art & Culture"
            },
            new Experience
            {
                Title = "Dubai Dune Safari",
                Slug = "dubai-dune-safari",
                Category = ExperienceCategoryKind.NatureEscapes,
                Region = RegionKind.MiddleEast,
                ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuDNef5AQO7oDv0c5b27USkQz8EotzQhYWWIPz1PbC4AY8SWRxxfu4oXVnZaPxKw1qZUIfrgRFVBgCSeJJN1DmNnxBo0PE2Y1_ksx_PvjvneSN6b9OcrP01OYDI-zr8isiqSbMD32Kzxv4td3Dl6-Np3OhJOaa-tS_eRxLGetMI97JlnacRAr4FM_y5fQlwMTYhGlD32QZC2muLsF_Jti5JZIkZ6DabGns1gEbskt8jl6xAsK5JYPY90SClP0n0ktrkN3dORoTFpDGA",
                Summary = "Golden dunes, private camp, and starlit dining.",
                LocationLabel = "Dubai, UAE",
                Rating = 4.85,
                PriceHint = "$1,100",
                BreadcrumbRegion = "Middle East",
                BreadcrumbCity = "Dubai",
                BreadcrumbCurrent = "Desert",
                TagLine = "Adventure"
            },
            new Experience
            {
                Title = "Fushimi Inari Twilight",
                Slug = "fushimi-inari-twilight",
                Category = ExperienceCategoryKind.HistoricalSites,
                Region = RegionKind.Asia,
                ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuAMEw_cYbTeAKBSDRFeCtJtK8TtX1mlk8hQrQBU7_Y3YVvEJgEpaLCMsw16yjkgewaXGv3PF58ppstAohdmzYr7EpIEuOtpVCQylmt2bq_K1qFCDSYejNjd2t7AscMtNtlnfp5isBXRlb_k2aCiUauHyFBLp7vkcBcD8duzNOGZcu2EXE3OdwUngG2hS2rV6CNTMw52hdzCInG_eSXwM9Ui4kyRXAut4wt99vlao195uz4EXdkF6jOS0dCGuzNStR9xNKvQVhpTDe4",
                Summary = "Lantern-lit torii paths at blue hour.",
                LocationLabel = "Kyoto, Japan",
                Rating = 4.95,
                PriceHint = "$125",
                BreadcrumbRegion = "Asia",
                BreadcrumbCity = "Kyoto",
                BreadcrumbCurrent = "Fushimi Inari",
                TagLine = "Historical"
            },
            new Experience
            {
                Title = "Holistic Wellness Retreat",
                Slug = "holistic-wellness",
                Category = ExperienceCategoryKind.NatureEscapes,
                Region = RegionKind.Asia,
                ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuCNXxIIMkoa-scGap_NvRfRuuX9LUvlAHrDSJsK1XwxpwyBt8FbMwV5LOFrrImZcIBUsqCA5VBxcG6Uu5grHeO3b8NbF99fZnP2UVlojqlSM2krj-vvAcRmOD8uVmcPh35kZB8_N8YmoEdQvQiT_L6HeyLKtZbtccsLBhfJLHgZCcZG6ItWOj9-I3Jv3bO9hJNdNEnZNbPGj_-qJ8NWBB6a2DkBCd-r_QdIBPDQdUWEhhAZpf2y7NW_uvBXz2-BJTedKno08LvYegQ",
                Summary = "Jungle sanctuary yoga and curated nourishment.",
                LocationLabel = "Bali, Indonesia",
                Rating = 4.9,
                PriceHint = "$300",
                BreadcrumbRegion = "Asia",
                BreadcrumbCity = "Bali",
                BreadcrumbCurrent = "Sanctuary",
                TagLine = "Wellness"
            },
            new Experience
            {
                Title = "MoMA After Hours",
                Slug = "nyc-moma-after",
                Category = ExperienceCategoryKind.ArtCulture,
                Region = RegionKind.Americas,
                ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuDYp_DJyD6Q5PrMRhxdoAVkmloppOgfHcNyvnOqYbIsqQxCcgqlfi-pyvzPGSQmOfLqVKlRFi5YiCfUZg1iUCLrOEjdU5ZS2DlD9G2zHSbb_IqXJFRXVAeWng9n-KjuicBdmsk2YgzPAl7PyMsdkUaNW29123kdRQwpizfDifVqdjn2ShMZVKWvDzjZYbb0RClkNZMomtLVqJ-3nc9e3uf8c256ddTgEjmDmdmoWr6r6tP0ORNvymec15FrxazZLhpcaxxT0XHz1HU",
                Summary = "Private curator walkthrough and skyline rooftop caps the evening.",
                LocationLabel = "New York, USA",
                Rating = 4.8,
                PriceHint = "$540",
                BreadcrumbRegion = "Americas",
                BreadcrumbCity = "New York",
                BreadcrumbCurrent = "MoMA",
                TagLine = "Art & Culture"
            });

        if (!await ctx.Tours.AnyAsync())
            ctx.Tours.AddRange(
            new Tour { Title = "Grand Canyon Expedition", Slug = "grand-canyon-expedition", Price = 1299, DurationDays = 5, CategoryLabel = "Adventure", ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuAAMJP5QjiSuZ_PAaGkhEVC1VehoywPT9HArsjiKJ8lM3W_FPT6-yLFgoMzsZX4mvGv-9DsbyWLHHNlppDDR50HtmgU40xxSofaSo0ej8i1CzjVgTep15PsKYaV1OvdbAfqXYxO90imUTQpiP0YvMnw4chdHLqlREEEjrXadRXw5roKl9_HCfSgftKZvPrSelTM5rFKs-6LUV0KnhZX4XmLa6oN_QKNNDGxDUw_Pc9k9YnmBEtHnDreA1tQH2njyRCoFQt2wVjMkS4", Summary = "Expert-led canyon trails and scenic vistas.", Rating = 4.9, ReviewCount = 128 },
            new Tour { Title = "Kyoto Heritage Walk", Slug = "kyoto-heritage-walk", Price = 850, DurationDays = 3, CategoryLabel = "Cultural", ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuAMEw_cYbTeAKBSDRFeCtJtK8TtX1mlk8hQrQBU7_Y3YVvEJgEpaLCMsw16yjkgewaXGv3PF58ppstAohdmzYr7EpIEuOtpVCQylmt2bq_K1qFCDSYejNjd2t7AscMtNtlnfp5isBXRlb_k2aCiUauHyFBLp7vkcBcD8duzNOGZcu2EXE3OdwUngG2hS2rV6CNTMw52hdzCInG_eSXwM9Ui4kyRXAut4wt99vlao195uz4EXdkF6jOS0dCGuzNStR9xNKvQVhpTDe4", Summary = "Shrines, gardens, and tea ceremony in Gion.", Rating = 4.8, ReviewCount = 94 },
            new Tour { Title = "Maldives Retreat", Slug = "maldives-retreat", Price = 2450, DurationDays = 7, CategoryLabel = "Luxury", ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuCNXxIIMkoa-scGap_NvRfRuuX9LUvlAHrDSJsK1XwxpwyBt8FbMwV5LOFrrImZcIBUsqCA5VBxcG6Uu5grHeO3b8NbF99fZnP2UVlojqlSM2krj-vvAcRmOD8uVmcPh35kZB8_N8YmoEdQvQiT_L6HeyLKtZbtccsLBhfJLHgZCcZG6ItWOj9-I3Jv3bO9hJNdNEnZNbPGj_-qJ8NWBB6a2DkBCd-r_QdIBPDQdUWEhhAZpf2y7NW_uvBXz2-BJTedKno08LvYegQ", Summary = "Overwater villas and coral snorkeling.", Rating = 5.0, ReviewCount = 215 },
            new Tour { Title = "Parisian Romance", Slug = "parisian-romance", Price = 1100, DurationDays = 4, CategoryLabel = "City Break", ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuDYp_DJyD6Q5PrMRhxdoAVkmloppOgfHcNyvnOqYbIsqQxCcgqlfi-pyvzPGSQmOfLqVKlRFi5YiCfUZg1iUCLrOEjdU5ZS2DlD9G2zHSbb_IqXJFRXVAeWng9n-KjuicBdmsk2YgzPAl7PyMsdkUaNW29123kdRQwpizfDifVqdjn2ShMZVKWvDzjZYbb0RClkNZMomtLVqJ-3nc9e3uf8c256ddTgEjmDmdmoWr6r6tP0ORNvymec15FrxazZLhpcaxxT0XHz1HU", Summary = "Museums, culinary masterclasses, and Seine evenings.", Rating = 4.7, ReviewCount = 182 },
            new Tour { Title = "Bali Wellness Escape", Slug = "bali-wellness-escape", Price = 720, DurationDays = 6, CategoryLabel = "Nature", ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuBdUbjFf_Ay4_sbeROeCm4qj0zstPYCGCM6V9lY1MQYH2osYeQ5cQu7FDoQccbGetLFPPI1qq7cI_WsNZxfaEJquymiDTQpYwL-dTou7ihFlbweRNvXoIB7C0CSm9Yn7-JJbQeNqY6uHCb5d6tBYCtAH6tAre5Qce9wEuf6o-kLYOtnfHz0dgGnAAU-4oDvct449czcGfMjrDvsErnjzLt7RNqUN42_eN57wU7tjBjVKFIjBhqUhfvWH3vlK0orCLfscLI3l2lmX5A", Summary = "Yoga, meditation, and organic dining.", Rating = 4.9, ReviewCount = 85 },
            new Tour { Title = "Swiss Alps Express", Slug = "swiss-alps-express", Price = 1850, DurationDays = 8, CategoryLabel = "Scenic", ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuDbLXtWvhF5N20g780zjl-31NiChKs-438Ld3x1-gKeHtCR2Mh5IdTAqXZhFWD8d9Htlw5zNFIpJHWCHWqRK1lQdjXybDij9ropLnjjLTv24lUDkoQl0mMT5nhivWIkNaeHrqmeWDXZGME2jTKe-B2g5xIXd2wpgcmoUoLs6T9k2H8hny4SIWP6qIBL1mbXwyjp_rPDBLkGmLy3oBoPJTtJcRPw7i6VgkHWy41OwB2ZN6cmCavrZkmLzcMHZZFDlGVV5sRBvHaFm4E", Summary = "Panoramic trains and alpine resorts.", Rating = 4.9, ReviewCount = 310 });

        if (!await ctx.Stays.AnyAsync())
            ctx.Stays.AddRange(
            new Stay { Name = "Azure Horizon Resort", Slug = "azure-horizon-resort", PricePerNight = 250, StarRating = 5, Region = RegionKind.Europe, CityLine = "Santorini, Greece", ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuC9LpP-KklBMXvFXlveAC3pdDQD6GT3SBOk4CSMXe7lj9TFTnMiAVf3tNa9klzChTq4ucCubzXRC6xNFeqcHxlSJwKqWiU528gUqTRmjRXRkr7s3Gs-tt3XWqA19MeGPcsAcNi1UoS5Cc71iNkCqzVTMhce8ZV24gnbOk_TWjKPJLsyFGYjjT4VcQAUq7ULLmT-iVyOdWOckaqwtsu-8cEpN8x68OlGgTjFdYlj3ooJiXVrAxr0hxtVgCqUFG1wOF2tK-UuiElDsSo", AmenitiesCsv = "wifi,pool,dining" },
            new Stay { Name = "The Metropolis Suites", Slug = "metropolis-suites", PricePerNight = 180, StarRating = 4, Region = RegionKind.Asia, CityLine = "Tokyo, Japan", ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuCJ-8PW2KpxsSkp3pvFyouKUDnLU2IBRSOllgJh4OKkzEinFnJPMTNEb2mx8sKiZKLcEfkP_VcSbCDYMSBEejGZCceY097AatBGQq3z2YVuS8XgajAmnSKxAFmlefONgiT9R_oif2s9gIMyJKgTk1P5rqN5AjD-wohpVVpgMy-aJtEguR4Qu1U6kBtYAAH0t7ODIkbQTMbtF5fX6Yx4OFEPIBZ93C-jvd_crTBz5PrGYNKENnk2m1UvRZc37j9O1OTmz3V4U8czzgE", AmenitiesCsv = "wifi,gym,cafe" },
            new Stay { Name = "Emerald Cove Villas", Slug = "emerald-cove-villas", PricePerNight = 550, StarRating = 5, Region = RegionKind.Asia, CityLine = "Bora Bora, French Polynesia", ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuBCX_y4PXoBjbcPZ6rK3lSQG0f13P9N1m6mh9rFjYfarVx1b7k9kZ-ATscOp4JEJTuKHoKzFeBad9LUyOcjO2iD0Ef8GGez6Rdt5KVQP5oddrR2sfvdcpyg7MSeZB25c9jQe9MEcESIA6L3I2tFBnNMDUQ36s8CLaR9pqJlArXvNCJYLvOgjNS2aQcE73aX9NksauHTvENMLoT37HHhSIJBgL6GWTKwVMOOwYbZlGJrbDr52H0L0e4ciU7T_Tt-AGsjdlgPd9PS1bM", AmenitiesCsv = "wifi,spa,pool" },
            new Stay { Name = "The Grand Imperial", Slug = "grand-imperial", PricePerNight = 320, StarRating = 5, Region = RegionKind.Europe, CityLine = "Vienna, Austria", ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuCspA9nivE7Z5w9HT4yw17NY_OYB2xEhGWU5lDtKzvG5pPIQpRPr_3c6kS193am78kpQ9D5kvm8Snwiwyn8fQ9hWa1schihorZqkIOe8GiVP0_6WWB5HXA6NQME73nsIiChrrprV6drGJoLWVL8K1eLuuN98TvRv1RlvmS07VY5YKES2_ryVVVUTdGunmgX63Ka6EtKrXOd_1ezKZtYjXM4ymGzf9QN-RJwzura65X7lkzpH9W_X6bRR4HJSRnV8bA0NbOE8AmsG7k", AmenitiesCsv = "wifi,dining,bar" },
            new Stay { Name = "Alpine Ridge Lodge", Slug = "alpine-ridge-lodge", PricePerNight = 290, StarRating = 4, Region = RegionKind.Americas, CityLine = "Whistler, Canada", ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuCpdLN_wxJ8Nuw8lA2zTCP1B9PdSYP9VC8y-CPaEROjghfco2poY-msDcRJahrRW4O70K1N1Wnikol7nK8UxnlE2UQ6ACxHxeqEbIXDsHucgCIE2I-20fGAVP0FdxjTXYB1_ECOwO2TlQ5fzTkjLYoQMWc5ulHP9lvfe--Ow73nZbAYakwfvpKXh4YHSnhxpVyUs9xL8-xU19UF-MnDcyeQEDof_c_BDXxJyFvf8TKEFFlY8V0_KsprxQO62jG_jwR9C3Bpfdnr7sI", AmenitiesCsv = "wifi,spa,gym" },
            new Stay { Name = "Jungle Canopy Retreat", Slug = "jungle-canopy-retreat", PricePerNight = 210, StarRating = 4, Region = RegionKind.Asia, CityLine = "Ubud, Bali", ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuBkRLQ6PWu4Hoecl6OtMu1OsiWZv1mcvHVBuicR4EMODonkjHT0tvM3KdNyoLUIfZL91KMa0b1g7tMz11WqAxdXrbNYptNXDsG4jQjqtkwtymHbo1ONCyt2p9BjGKGIq0KHD53Hys-nElgCj8Upwblz0bbNFdGmLsF9v7VQ4XkBcGejhl-RpLNKKCxRxQfAwBoQjr9uprNTTRekj2CxCulm_NUuLCi582I-6rGhyCJSYhhVgHefrHP8gTtKwopbq0d0ZlNEsK3PBVM", AmenitiesCsv = "spa,yoga,eco" });

        if (!await ctx.Attractions.AnyAsync())
        {
            ctx.Attractions.AddRange(
                new Attraction
                {
                    Name = "Eiffel Tower",
                    City = "Paris",
                    Category = "Landmark",
                    ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuCpeAkgq_-ebuCKW2L0CKwBJg8_y5EeriFpo9WUFj8srWgZjzKd_sa9HiaDidaWUHmkwzjX1DpwwBJ5ot2K5EQo--jpjLdMfkjh5iYTnFd1L4td9Fgm6i-lIbbQKpljUB3vJfHG8rYj00Z0A8j1IaVgaoK6kZeuXjXPuaHNdY8wnnfJu644SE-xQlyZqhAGcsdSRhn6xPRiPpEpyAK7v6MTRPUPgYHmobIrijKNWjn9PjzhcCNzVpA3x01LZEnZCR-nPQJoo5YwWzk"
                },
                new Attraction
                {
                    Name = "Colosseum",
                    City = "Rome",
                    Category = "Historical",
                    ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuBH39_lRZe2DJE9WWQcGWP3gcx-8-GoHdxrL2thlartCd869m9aZBIC_nfyvcG2_A0EsDOpa_UOZtiJFi2aIPT-NsFVXbZivL0J1XSMpo_9vnHepzVzTmux9iZJD7wsDT9Jc9j_zlEJHrSIxYzlaG0WKanmcSwGYzGKPU8g5kwgwMTD8F5mBgHe_Fi_KFMHdSKrdab558Rukau_vF-FxIonVndfmbcU4Rtcqpz20XVMjaUwntOCtQQONbtov1EiayPiLsWrRmALlORc"
                },
                new Attraction
                {
                    Name = "Louvre Museum",
                    City = "Paris",
                    Category = "Museum",
                    ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuDqNxX9udBDTYh_AvfJJMTq3DBTqMBLYBEZeWk5s3rhO-xNaQKqJ0_scdhN_Izt1FZ1I5uq79MxxwHTSuHT2NkceYumVDBfl8yvUG0L7DgQjM1euufAuxSUJFjzR2HnH4MOema6oJxncrugucBV5uw73ggJ4UyaZMawNBzOZCA10APLLsL0vpHf-s8zMa67BMzKMgKCEPpibVm8Spa6t_UciI-LdFxnuUnZ5fugyk_4mx8bNm6Ezzu9QQrj1uYLd_EjXPv_vrItchs"
                },
                new Attraction
                {
                    Name = "Central Park",
                    City = "New York",
                    Category = "Nature",
                    ImageUrl = null
                });
        }

        await ctx.SaveChangesAsync();

        await ExperienceCatalogRepair.ApplyAsync(ctx);
    }
}
