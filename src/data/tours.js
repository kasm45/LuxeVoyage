export const tours = [
  {
    id: 'dolomites-ring',
    title: 'Dolomites Grand Circuit',
    region: 'Europe',
    category: 'Adventure',
    days: 7,
    priceFrom: 2890,
    image:
      'https://lh3.googleusercontent.com/aida-public/AB6AXuCIR4IjC5A9zYCeVpv-6E2Qsr-6XGwJt_c_ahEjpLc8ZwVjHtz_S8_ABYHEdi18syPgvEJC77vQYcnvKvAVrnJ-5bRklkm1OeYVO9jxHTy-VhSEjEjkfWAigDBqqn8KKjuyysokk89K1Z1lCxhl95xAyFVMl6o9qx6AXkGfZotuG2vff5RbIOgYiqc3ZFT-zAe5gggKvE6s6bvZ7OB012-vm0Lg_HGyIo-JXxaYx54YHQId_hs6eJfYg3MkrDG_aEW5LUR8x2juAyQ',
    excerpt: 'Alpine passes, rifugio nights, and valley vineyards.',
    description:
      'Small-group pacing with private transfers, expert mountain guides, and farm-to-table evenings.',
  },
  {
    id: 'kyoto-heritage',
    title: 'Kyoto Heritage & Crafts',
    region: 'Asia',
    category: 'Culture',
    days: 5,
    priceFrom: 2140,
    image:
      'https://lh3.googleusercontent.com/aida-public/AB6AXuDMVPZyQmPXZnx0PYtp9I8Q54m5ROPjfQ0JfNsiZk1mbK9AN9Y53k6zKlVF-tSj6Cl9Wjb1JbQZfbZep7U5Z5K0h2r0D3lqLsCPoeaH01iCauiy2mysXS__FIAwLBd9xu9Vi2p0TYhBHEAhpxKUogWBhsy7baYglaDf8GSNv5Iyiwi5SSL6gK9hkIrCKPQxebVIPWX39ru9ZjCN_eW_9e3cdaJ9rP0f8-2Q3ku1H6xLw_qaZczVrCdPP-gwBFunKY8eS93bWaVTkDE',
    excerpt: 'Temple dawn, tea masters, and artisan ateliers.',
    description:
      'Stay in a restored machiya, learn from craftspeople, and dine with geiko-hosted evenings.',
  },
  {
    id: 'aegean-islands',
    title: 'Aegean Islands Slo-Mo',
    region: 'Europe',
    category: 'Coastal',
    days: 10,
    priceFrom: 4520,
    image:
      'https://lh3.googleusercontent.com/aida-public/AB6AXuA-o1yXyiJMLrcfTR49sat4bsjhr4zuvg13D_TF68Ef7BB4voeBpsvIbjR5ppokk_ThlHkR9waRXruwceHzG9iw8quOlUk8IjmSCUPgmAcNU9mFNtLMcVU4PXeONxKptg9klNWAixbea39voBtdgD_MggZHJj0Zb2_1t8p95oqoFp_SVOlxflvBnepA3iD5vFT6Ai6DXB0SjuqZ337maKwHXwu0mv71e3_XNHoGygkrcIWQCer6qeKNgxBwF3tjXa4DJMOcQm-e4PQ',
    excerpt: 'Ferry-hopping with private chefs and hidden coves.',
    description:
      'Skip crowded ports; anchor at Cycladic gems with curated seaside villas each stop.',
  },
  {
    id: 'patagonia-expedition',
    title: 'Patagonia Ice & Wind',
    region: 'Americas',
    category: 'Adventure',
    days: 9,
    priceFrom: 5280,
    image:
      'https://lh3.googleusercontent.com/aida-public/AB6AXuBfLmPPvRhBGcmEobeHhHO2a_7h3knpyF4eiuW5aZdkCyYcsB0Z-qb17FEXLnEeYuUqBeH0vl690tP127JT9xggNqhPlFLMHU0ClCvTtSBIkduweCdWpBHiq63KiXPP2ul1psaTeQQR-U4alttTY_BS2h_ek1d_EGIzLc0awsNarTDMvXgqcMcgogxod07JTOuubpA3042YuMsHetKdqjEy8HYCw1lwhS_SdnXrYrPwaYFQpjcjbtYFjy5TWRpp4kRh3AVKgUObTRY',
    excerpt: 'Glacier trekking, estancia life, and dark-sky nights.',
    description:
      'Domestic flights included; eco-lodges with naturalists and flexible weather routing.',
  },
];

export function getTourById(id) {
  return tours.find((t) => t.id === id);
}
