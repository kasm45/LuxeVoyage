export const hotels = [
  {
    id: 'cliffside-amalfi',
    name: 'Villa Maris, Amalfi',
    city: 'Amalfi',
    region: 'Europe',
    category: 'Luxury',
    priceLevel: 4,
    priceFrom: 890,
    rating: 4.9,
    image:
      'https://lh3.googleusercontent.com/aida-public/AB6AXuA-o1yXyiJMLrcfTR49sat4bsjhr4zuvg13D_TF68Ef7BB4voeBpsvIbjR5ppokk_ThlHkR9waRXruwceHzG9iw8quOlUk8IjmSCUPgmAcNU9mFNtLMcVU4PXeONxKptg9klNWAixbea39voBtdgD_MggZHJj0Zb2_1t8p95oqoFp_SVOlxflvBnepA3iD5vFT6Ai6DXB0SjuqZ337maKwHXwu0mv71e3_XNHoGygkrcIWQCer6qeKNgxBwF3tjXa4DJMOcQm-e4PQ',
    excerpt: 'Infinity pool draped over the Tyrrhenian Sea.',
    amenities: ['Butler', 'Michelin chef', 'Private dock'],
  },
  {
    id: 'tokyo-sky',
    name: 'Hansei Tower Suites',
    city: 'Tokyo',
    region: 'Asia',
    category: 'Design',
    priceLevel: 3,
    priceFrom: 420,
    rating: 4.8,
    image:
      'https://lh3.googleusercontent.com/aida-public/AB6AXuDMVPZyQmPXZnx0PYtp9I8Q54m5ROPjfQ0JfNsiZk1mbK9AN9Y53k6zKlVF-tSj6Cl9Wjb1JbQZfbZep7U5Z5K0h2r0D3lqLsCPoeaH01iCauiy2mysXS__FIAwLBd9xu9Vi2p0TYhBHEAhpxKUogWBhsy7baYglaDf8GSNv5Iyiwi5SSL6gK9hkIrCKPQxebVIPWX39ru9ZjCN_eW_9e3cdaJ9rP0f8-2Q3ku1H6xLw_qaZczVrCdPP-gwBFunKY8eS93bWaVTkDE',
    excerpt: 'Floor-to-ceiling skyline above Shibuya crossing.',
    amenities: ['Sky lounge', 'Onsen bath', 'Club floor'],
  },
  {
    id: 'zermatt-chalet',
    name: 'Chalet Arête',
    city: 'Zermatt',
    region: 'Europe',
    category: 'Chalet',
    priceLevel: 4,
    priceFrom: 1240,
    rating: 5,
    image:
      'https://lh3.googleusercontent.com/aida-public/AB6AXuCIR4IjC5A9zYCeVpv-6E2Qsr-6XGwJt_c_ahEjpLc8ZwVjHtz_S8_ABYHEdi18syPgvEJC77vQYcnvKvAVrnJ-5bRklkm1OeYVO9jxHTy-VhSEjEjkfWAigDBqqn8KKjuyysokk89K1Z1lCxhl95xAyFVMl6o9qx6AXkGfZotuG2vff5RbIOgYiqc3ZFT-zAe5gggKvE6s6bvZ7OB012-vm0Lg_HGyIo-JXxaYx54YHQId_hs6eJfYg3MkrDG_aEW5LUR8x2juAyQ',
    excerpt: 'Ski-in lodge with Matterhorn terraces.',
    amenities: ['Private chef', 'Cinema', 'Spa suite'],
  },
  {
    id: 'santorini-cave',
    name: 'Caldera Echo Suites',
    city: 'Oía',
    region: 'Europe',
    category: 'Boutique',
    priceLevel: 3,
    priceFrom: 560,
    rating: 4.7,
    image:
      'https://lh3.googleusercontent.com/aida-public/AB6AXuBmuxz78CkUOTf5-2i-K-RA3SRKhZP0sQOPSNKZzrmyEwu8oLqJpQG6x6Mfym3dnnHxXx2g9TQkXA1Clvv-XI2lvoJXEdh8jaqGFhm5LuSC2Mb3gQxMxhCTyvj_Grfw6cxxBHdHV3YvV8Kln4kiivHHvHzcClnjwLBPWhaV5kFpGjhyjS6ULK8Wzpo6hqkTcvre-RpRnza8Xuh5q0n9Df6vgUzkdbo6U8ZSEOkurVGq5WPTb7Cps2vuto6xjjoWAf2OAfhfu254U-k',
    excerpt: 'Cave pools carved into the volcanic rim.',
    amenities: ['Private plunge', 'Sunset deck', 'Wine cellar'],
  },
];

export function getHotelById(id) {
  return hotels.find((h) => h.id === id);
}
