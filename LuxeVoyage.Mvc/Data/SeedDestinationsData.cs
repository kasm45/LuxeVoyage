using LuxeVoyage.Mvc.Models;

namespace LuxeVoyage.Mvc.Data;

/// <summary>Canonical catalog destinations inserted by <see cref="DbInitializer"/> when missing (matched by <see cref="Destination.Title"/>).</summary>
public static class SeedDestinationsData
{
    // Reuse polished imagery from existing catalog seeds where possible.
    private const string ImgAmalfi =
        "https://lh3.googleusercontent.com/aida-public/AB6AXuCArzoJK4wGykjJ3kxui9HP-8-Q4aQlsQtehSUkaqULNCJJsd-b7oy4-8DJeFMQFUlfZbWVUS3tEwMuGHdyILFR9BFbWdPJJJWu0lMZg6i7ti7hOhWRmJ2U6UXpMkHOzR55MzKv3LBEwmj9wfvRMsZOjrIySl7LB9_JoX6gm9nYfAN7cZOyO8ohWWuC8o_8qfS8TzAD16iYSlGWS-PTFnoWN81jJcFAscv-3Rb8dcA1lRDkl2cMbuX4iR_sI2t_TBffgMvgZ5wPczA";
    private const string ImgTokyo =
        "https://lh3.googleusercontent.com/aida-public/AB6AXuCr_nneCcikM9-e1Ywb85MHku6fL3aytqO14CGoX9Ukw3lD_Z9PvLfLivOeP9AFBmSAgU7cHwRLj9L897-lB5bpNJtfzBa4lPKhJ48eL_rMM46c4qjB28fEeAX3FgHCU77CktH2503Logy0EpgI3A4DRULYXDBR9wBLwF4KAVkbRZLl1zQYFs1tnXIzD0rVl_TUpww0yoQ6xkYrqb-hwD6G09rREUGYJSj1jwrNM-KPT_IAbHzfu9s9A5xMLTeJp2Ex6u7dPJNhRQ0";
    private const string ImgAlps =
        "https://lh3.googleusercontent.com/aida-public/AB6AXuDbLXtWvhF5N20g780zjl-31NiChKs-438Ld3x1-gKeHtCR2Mh5IdTAqXZhFWD8d9Htlw5zNFIpJHWCHWqRK1lQdjXybDij9ropLnjjLTv24lUDkoQl0mMT5nhivWIkNaeHrqmeWDXZGME2jTKe-B2g5xIXd2wpgcmoUoLs6T9k2H8hny4SIWP6qIBL1mbXwyjp_rPDBLkGmLy3oBoPJTtJcRPw7i6VgkHWy41OwB2ZN6cmCavrZkmLzcMHZZFDlGVV5sRBvHaFm4E";
    private const string ImgMedina =
        "https://lh3.googleusercontent.com/aida-public/AB6AXuDNef5AQO7oDv0c5b27USkQz8EotzQhYWWIPz1PbC4AY8SWRxxfu4oXVnZaPxKw1qZUIfrgRFVBgCSeJJN1DmNnxBo0PE2Y1_ksx_PvjvneSN6b9OcrP01OYDI-zr8isiqSbMD32Kzxv4td3Dl6-Np3OhJOaa-tS_eRxLGetMI97JlnacRAr4FM_y5fQlwMTYhGlD32QZC2muLsF_Jti5JZIkZ6DabGns1gEbskt8jl6xAsK5JYPY90SClP0n0ktrkN3dORoTFpDGA";
    private const string ImgPatagonia =
        "https://lh3.googleusercontent.com/aida-public/AB6AXuAAMJP5QjiSuZ_PAaGkhEVC1VehoywPT9HArsjiKJ8lM3W_FPT6-yLFgoMzsZX4mvGv-9DsbyWLHHNlppDDR50HtmgU40xxSofaSo0ej8i1CzjVgTep15PsKYaV1OvdbAfqXYxO90imUTQpiP0YvMnw4chdHLqlREEEjrXadRXw5roKl9_HCfSgftKZvPrSelTM5rFKs-6LUV0KnhZX4XmLa6oN_QKNNDGxDUw_Pc9k9YnmBEtHnDreA1tQH2njyRCoFQt2wVjMkS4";
    private const string ImgKyoto =
        "https://lh3.googleusercontent.com/aida-public/AB6AXuAMEw_cYbTeAKBSDRFeCtJtK8TtX1mlk8hQrQBU7_Y3YVvEJgEpaLCMsw16yjkgewaXGv3PF58ppstAohdmzYr7EpIEuOtpVCQylmt2bq_K1qFCDSYejNjd2t7AscMtNtlnfp5isBXRlb_k2aCiUauHyFBLp7vkcBcD8duzNOGZcu2EXE3OdwUngG2hS2rV6CNTMw52hdzCInG_eSXwM9Ui4kyRXAut4wt99vlao195uz4EXdkF6jOS0dCGuzNStR9xNKvQVhpTDe4";
    private const string ImgParis =
        "https://lh3.googleusercontent.com/aida-public/AB6AXuCHnkC4jqzoSm6c-wP1_id_3vgsakis6jVASXJ569o81eqQHWQNYFR3iPVgz0IOTghlkyjf7j7FMJqO3VeU2eEK30Ov1vHp-eWi8kHVcmKwYhg6cr88dEdZ5539y_UJ6ab9MWrsmM3-pIzm6FyCT74rrTq2L1PF7CfbKA2vUb7_cFHgBd-y59p2EuuoQkD0W3E2vH-C6X1qfCcw6aVsewT79jVi30DWH7impJxUrzfnmfbmUDpv3qPfvAI-nFIBpTq-_T0XJTznNZQ";
    private const string ImgBeach =
        "https://lh3.googleusercontent.com/aida-public/AB6AXuCNXxIIMkoa-scGap_NvRfRuuX9LUvlAHrDSJsK1XwxpwyBt8FbMwV5LOFrrImZcIBUsqCA5VBxcG6Uu5grHeO3b8NbF99fZnP2UVlojqlSM2krj-vvAcRmOD8uVmcPh35kZB8_N8YmoEdQvQiT_L6HeyLKtZbtccsLBhfJLHgZCcZG6ItWOj9-I3Jv3bO9hJNdNEnZNbPGj_-qJ8NWBB6a2DkBCd-r_QdIBPDQdUWEhhAZpf2y7NW_uvBXz2-BJTedKno08LvYegQ";

    public static IReadOnlyList<Destination> All => new[]
    {
        new Destination
        {
            Title = "Old Town Amalfi",
            Slug = "amalfi-coast",
            Category = ExperienceCategoryKind.HistoricalSites,
            Region = RegionKind.Europe,
            ImageUrl = ImgAmalfi,
            Summary = "Sun-drenched cobblestones and coastal heritage.",
            LocationLabel = "Italy",
            Rating = 4.9,
            PriceHint = "$420",
            BreadcrumbRegion = "Europe",
            BreadcrumbCity = "Campania",
            BreadcrumbCurrent = "Amalfi Coast",
            TagLine = "History"
        },
        new Destination
        {
            Title = "Tokyo City Lights",
            Slug = "tokyo",
            Category = ExperienceCategoryKind.CulinaryJourneys,
            Region = RegionKind.Asia,
            ImageUrl = ImgTokyo,
            Summary = "Neon skylines and intimate chef-led tastings.",
            LocationLabel = "Japan",
            Rating = 4.8,
            PriceHint = "$310",
            BreadcrumbRegion = "Asia",
            BreadcrumbCity = "Tokyo",
            BreadcrumbCurrent = "Tokyo",
            TagLine = "Culinary"
        },
        new Destination
        {
            Title = "Swiss Alpine Vista",
            Slug = "zermatt",
            Category = ExperienceCategoryKind.NatureEscapes,
            Region = RegionKind.Europe,
            ImageUrl = ImgAlps,
            Summary = "Glacier-carved peaks and crisp alpine air.",
            LocationLabel = "Switzerland",
            Rating = 4.95,
            PriceHint = "$580",
            BreadcrumbRegion = "Europe",
            BreadcrumbCity = "Valais",
            BreadcrumbCurrent = "Zermatt",
            TagLine = "Nature"
        },
        new Destination
        {
            Title = "Marrakech Medina",
            Slug = "marrakech-medina",
            Category = ExperienceCategoryKind.ArtCulture,
            Region = RegionKind.Africa,
            ImageUrl = ImgMedina,
            Summary = "Souks, riads, and artisan workshops.",
            LocationLabel = "Morocco",
            Rating = 4.7,
            PriceHint = "$260",
            BreadcrumbRegion = "Africa",
            BreadcrumbCity = "Marrakech",
            BreadcrumbCurrent = "Medina",
            TagLine = "Art & Culture"
        },
        new Destination
        {
            Title = "Patagonia Wilderness",
            Slug = "patagonia-wild",
            Category = ExperienceCategoryKind.NatureEscapes,
            Region = RegionKind.Americas,
            ImageUrl = ImgPatagonia,
            Summary = "Torres del Paine vistas and gaucho trails.",
            LocationLabel = "Chile",
            Rating = 4.85,
            PriceHint = "$490",
            BreadcrumbRegion = "Americas",
            BreadcrumbCity = "Patagonia",
            BreadcrumbCurrent = "Wilderness",
            TagLine = "Nature"
        },
        new Destination
        {
            Title = "Kyoto Heritage Quarter",
            Slug = "kyoto-quarter",
            Category = ExperienceCategoryKind.HistoricalSites,
            Region = RegionKind.Asia,
            ImageUrl = ImgKyoto,
            Summary = "Temple gates, moss gardens, and tea houses.",
            LocationLabel = "Japan",
            Rating = 4.9,
            PriceHint = "$340",
            BreadcrumbRegion = "Asia",
            BreadcrumbCity = "Kyoto",
            BreadcrumbCurrent = "Heritage",
            TagLine = "Historical"
        },
        new Destination
        {
            Title = "Paris, France",
            Slug = "paris-france",
            Category = ExperienceCategoryKind.ArtCulture,
            Region = RegionKind.Europe,
            ImageUrl = ImgParis,
            Summary = "Grand boulevards, world-class museums, and café culture.",
            LocationLabel = "Paris, France",
            Rating = 4.95,
            PriceHint = "$620",
            BreadcrumbRegion = "Europe",
            BreadcrumbCity = "Paris",
            BreadcrumbCurrent = "City Center",
            TagLine = "Art & Culture"
        },
        new Destination
        {
            Title = "Rome, Italy",
            Slug = "rome-italy",
            Category = ExperienceCategoryKind.HistoricalSites,
            Region = RegionKind.Europe,
            ImageUrl = ImgAmalfi,
            Summary = "Ancient forums, Renaissance piazzas, and Roman culinary tradition.",
            LocationLabel = "Rome, Italy",
            Rating = 4.9,
            PriceHint = "$480",
            BreadcrumbRegion = "Europe",
            BreadcrumbCity = "Rome",
            BreadcrumbCurrent = "Historic Core",
            TagLine = "History"
        },
        new Destination
        {
            Title = "Istanbul, Turkey",
            Slug = "istanbul-turkey",
            Category = ExperienceCategoryKind.HistoricalSites,
            Region = RegionKind.MiddleEast,
            ImageUrl = ImgMedina,
            Summary = "Where continents meet — bazaars, mosques, and the Bosphorus.",
            LocationLabel = "Istanbul, Turkey",
            Rating = 4.85,
            PriceHint = "$320",
            BreadcrumbRegion = "Middle East",
            BreadcrumbCity = "Istanbul",
            BreadcrumbCurrent = "Old City",
            TagLine = "Heritage"
        },
        new Destination
        {
            Title = "Cappadocia, Turkey",
            Slug = "cappadocia-turkey",
            Category = ExperienceCategoryKind.NatureEscapes,
            Region = RegionKind.MiddleEast,
            ImageUrl = ImgPatagonia,
            Summary = "Fairy chimneys, cave suites, and sunrise balloon flights.",
            LocationLabel = "Cappadocia, Turkey",
            Rating = 4.9,
            PriceHint = "$410",
            BreadcrumbRegion = "Middle East",
            BreadcrumbCity = "Göreme",
            BreadcrumbCurrent = "Valleys",
            TagLine = "Nature"
        },
        new Destination
        {
            Title = "Antalya, Turkey",
            Slug = "antalya-turkey",
            Category = ExperienceCategoryKind.NatureEscapes,
            Region = RegionKind.MiddleEast,
            ImageUrl = ImgBeach,
            Summary = "Turquoise coast, Roman harbors, and resort refinement.",
            LocationLabel = "Antalya, Turkey",
            Rating = 4.75,
            PriceHint = "$290",
            BreadcrumbRegion = "Middle East",
            BreadcrumbCity = "Antalya",
            BreadcrumbCurrent = "Riviera",
            TagLine = "Coastal"
        },
        new Destination
        {
            Title = "London, United Kingdom",
            Slug = "london-uk",
            Category = ExperienceCategoryKind.ArtCulture,
            Region = RegionKind.Europe,
            ImageUrl = ImgParis,
            Summary = "Royal parks, West End stages, and timeless Thames views.",
            LocationLabel = "London, United Kingdom",
            Rating = 4.85,
            PriceHint = "$540",
            BreadcrumbRegion = "Europe",
            BreadcrumbCity = "London",
            BreadcrumbCurrent = "Westminster",
            TagLine = "Urban"
        },
        new Destination
        {
            Title = "Barcelona, Spain",
            Slug = "barcelona-spain",
            Category = ExperienceCategoryKind.ArtCulture,
            Region = RegionKind.Europe,
            ImageUrl = ImgAmalfi,
            Summary = "Gaudí skylines, Mediterranean light, and tapas alleys.",
            LocationLabel = "Barcelona, Spain",
            Rating = 4.8,
            PriceHint = "$380",
            BreadcrumbRegion = "Europe",
            BreadcrumbCity = "Barcelona",
            BreadcrumbCurrent = "Gothic Quarter",
            TagLine = "Culture"
        },
        new Destination
        {
            Title = "Athens, Greece",
            Slug = "athens-greece",
            Category = ExperienceCategoryKind.HistoricalSites,
            Region = RegionKind.Europe,
            ImageUrl = ImgKyoto,
            Summary = "Acropolis light and Aegean breezes through marble streets.",
            LocationLabel = "Athens, Greece",
            Rating = 4.75,
            PriceHint = "$340",
            BreadcrumbRegion = "Europe",
            BreadcrumbCity = "Athens",
            BreadcrumbCurrent = "Plaka",
            TagLine = "Antiquity"
        },
        new Destination
        {
            Title = "Santorini, Greece",
            Slug = "santorini-greece",
            Category = ExperienceCategoryKind.NatureEscapes,
            Region = RegionKind.Europe,
            ImageUrl = ImgBeach,
            Summary = "Caldera sunsets and cliffside boutique serenity.",
            LocationLabel = "Santorini, Greece",
            Rating = 4.95,
            PriceHint = "$520",
            BreadcrumbRegion = "Europe",
            BreadcrumbCity = "Santorini",
            BreadcrumbCurrent = "Oia",
            TagLine = "Coastal"
        },
        new Destination
        {
            Title = "Dubai, UAE",
            Slug = "dubai-uae",
            Category = ExperienceCategoryKind.CulinaryJourneys,
            Region = RegionKind.MiddleEast,
            ImageUrl = ImgTokyo,
            Summary = "Desert futurism, rooftop dining, and waterfront promenades.",
            LocationLabel = "Dubai, UAE",
            Rating = 4.8,
            PriceHint = "$680",
            BreadcrumbRegion = "Middle East",
            BreadcrumbCity = "Dubai",
            BreadcrumbCurrent = "Marina",
            TagLine = "Luxury"
        },
        new Destination
        {
            Title = "Bali, Indonesia",
            Slug = "bali-indonesia",
            Category = ExperienceCategoryKind.NatureEscapes,
            Region = RegionKind.Asia,
            ImageUrl = ImgBeach,
            Summary = "Rice terraces, volcanic peaks, and rejuvenating retreats.",
            LocationLabel = "Bali, Indonesia",
            Rating = 4.9,
            PriceHint = "$360",
            BreadcrumbRegion = "Asia",
            BreadcrumbCity = "Ubud",
            BreadcrumbCurrent = "Island",
            TagLine = "Wellness"
        },
        new Destination
        {
            Title = "Bangkok, Thailand",
            Slug = "bangkok-thailand",
            Category = ExperienceCategoryKind.CulinaryJourneys,
            Region = RegionKind.Asia,
            ImageUrl = ImgKyoto,
            Summary = "Temple riverside life and vibrant night markets.",
            LocationLabel = "Bangkok, Thailand",
            Rating = 4.7,
            PriceHint = "$240",
            BreadcrumbRegion = "Asia",
            BreadcrumbCity = "Bangkok",
            BreadcrumbCurrent = "Chao Phraya",
            TagLine = "Culinary"
        },
        new Destination
        {
            Title = "New York, USA",
            Slug = "new-york-usa",
            Category = ExperienceCategoryKind.ArtCulture,
            Region = RegionKind.Americas,
            ImageUrl = ImgParis,
            Summary = "Broadway energy, skyline suites, and museum corridors.",
            LocationLabel = "New York, USA",
            Rating = 4.85,
            PriceHint = "$590",
            BreadcrumbRegion = "Americas",
            BreadcrumbCity = "New York",
            BreadcrumbCurrent = "Manhattan",
            TagLine = "Urban"
        },
        new Destination
        {
            Title = "Los Angeles, USA",
            Slug = "los-angeles-usa",
            Category = ExperienceCategoryKind.NatureEscapes,
            Region = RegionKind.Americas,
            ImageUrl = ImgTokyo,
            Summary = "Pacific coast drives, design studios, and hillside escapes.",
            LocationLabel = "Los Angeles, USA",
            Rating = 4.75,
            PriceHint = "$510",
            BreadcrumbRegion = "Americas",
            BreadcrumbCity = "Los Angeles",
            BreadcrumbCurrent = "Westside",
            TagLine = "Coastal"
        },
        new Destination
        {
            Title = "Rio de Janeiro, Brazil",
            Slug = "rio-de-janeiro-brazil",
            Category = ExperienceCategoryKind.NatureEscapes,
            Region = RegionKind.Americas,
            ImageUrl = ImgBeach,
            Summary = "Sugarloaf views, samba rhythms, and Atlantic shores.",
            LocationLabel = "Rio de Janeiro, Brazil",
            Rating = 4.8,
            PriceHint = "$430",
            BreadcrumbRegion = "Americas",
            BreadcrumbCity = "Rio de Janeiro",
            BreadcrumbCurrent = "Copacabana",
            TagLine = "Coastal"
        },
        new Destination
        {
            Title = "Cairo, Egypt",
            Slug = "cairo-egypt",
            Category = ExperienceCategoryKind.HistoricalSites,
            Region = RegionKind.Africa,
            ImageUrl = ImgMedina,
            Summary = "Pyramids at dawn and Nile-side grandeur.",
            LocationLabel = "Cairo, Egypt",
            Rating = 4.65,
            PriceHint = "$280",
            BreadcrumbRegion = "Africa",
            BreadcrumbCity = "Cairo",
            BreadcrumbCurrent = "Giza",
            TagLine = "History"
        },
        new Destination
        {
            Title = "Cape Town, South Africa",
            Slug = "cape-town-south-africa",
            Category = ExperienceCategoryKind.NatureEscapes,
            Region = RegionKind.Africa,
            ImageUrl = ImgPatagonia,
            Summary = "Table Mountain drama and Cape Winelands escapes.",
            LocationLabel = "Cape Town, South Africa",
            Rating = 4.85,
            PriceHint = "$390",
            BreadcrumbRegion = "Africa",
            BreadcrumbCity = "Cape Town",
            BreadcrumbCurrent = "Waterfront",
            TagLine = "Nature"
        },
        new Destination
        {
            Title = "Sydney, Australia",
            Slug = "sydney-australia",
            Category = ExperienceCategoryKind.NatureEscapes,
            Region = RegionKind.Asia,
            ImageUrl = ImgBeach,
            Summary = "Harbor sails, coastal trails, and refined harborside stays.",
            LocationLabel = "Sydney, Australia",
            Rating = 4.8,
            PriceHint = "$470",
            BreadcrumbRegion = "Oceania",
            BreadcrumbCity = "Sydney",
            BreadcrumbCurrent = "Harbour",
            TagLine = "Coastal"
        },
        new Destination
        {
            Title = "Maldives",
            Slug = "maldives",
            Category = ExperienceCategoryKind.NatureEscapes,
            Region = RegionKind.Asia,
            ImageUrl = ImgBeach,
            Summary = "Overwater serenity and coral gardens in crystal lagoons.",
            LocationLabel = "Maldives",
            Rating = 4.95,
            PriceHint = "$890",
            BreadcrumbRegion = "Indian Ocean",
            BreadcrumbCity = "Atolls",
            BreadcrumbCurrent = "Resorts",
            TagLine = "Island"
        },
        new Destination
        {
            Title = "Bora Bora",
            Slug = "bora-bora",
            Category = ExperienceCategoryKind.NatureEscapes,
            Region = RegionKind.Asia,
            ImageUrl = ImgBeach,
            Summary = "Lagoon blues and iconic peak views from private villas.",
            LocationLabel = "French Polynesia",
            Rating = 4.98,
            PriceHint = "$1,200",
            BreadcrumbRegion = "Pacific",
            BreadcrumbCity = "Bora Bora",
            BreadcrumbCurrent = "Lagoon",
            TagLine = "Island"
        },
        new Destination
        {
            Title = "Swiss Alps",
            Slug = "swiss-alps",
            Category = ExperienceCategoryKind.NatureEscapes,
            Region = RegionKind.Europe,
            ImageUrl = ImgAlps,
            Summary = "Panoramic ridge lines, cog railways, and alpine wellness.",
            LocationLabel = "Switzerland",
            Rating = 4.92,
            PriceHint = "$610",
            BreadcrumbRegion = "Europe",
            BreadcrumbCity = "Bernese Oberland",
            BreadcrumbCurrent = "High Alps",
            TagLine = "Nature"
        },
        new Destination
        {
            Title = "Amsterdam, Netherlands",
            Slug = "amsterdam-netherlands",
            Category = ExperienceCategoryKind.ArtCulture,
            Region = RegionKind.Europe,
            ImageUrl = ImgParis,
            Summary = "Canal rings, design galleries, and refined cycling culture.",
            LocationLabel = "Amsterdam, Netherlands",
            Rating = 4.78,
            PriceHint = "$410",
            BreadcrumbRegion = "Europe",
            BreadcrumbCity = "Amsterdam",
            BreadcrumbCurrent = "Canal Belt",
            TagLine = "Culture"
        },
        new Destination
        {
            Title = "Prague, Czech Republic",
            Slug = "prague-czech-republic",
            Category = ExperienceCategoryKind.HistoricalSites,
            Region = RegionKind.Europe,
            ImageUrl = ImgKyoto,
            Summary = "Gothic spires, castle vistas, and cobbled Old Town lanes.",
            LocationLabel = "Prague, Czech Republic",
            Rating = 4.72,
            PriceHint = "$310",
            BreadcrumbRegion = "Europe",
            BreadcrumbCity = "Prague",
            BreadcrumbCurrent = "Old Town",
            TagLine = "Heritage"
        }
    };
}
