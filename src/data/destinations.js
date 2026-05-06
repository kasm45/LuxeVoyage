/** Mock destinations — LuxeVoyage style imagery from original UI */
export const destinations = [
  {
    id: 'amalfi-coast',
    name: 'Amalfi Coast, Italy',
    region: 'Europe',
    category: 'Coastal',
    priceLevel: 4,
    priceFrom: 420,
    image:
      'https://lh3.googleusercontent.com/aida-public/AB6AXuA-o1yXyiJMLrcfTR49sat4bsjhr4zuvg13D_TF68Ef7BB4voeBpsvIbjR5ppokk_ThlHkR9waRXruwceHzG9iw8quOlUk8IjmSCUPgmAcNU9mFNtLMcVU4PXeONxKptg9klNWAixbea39voBtdgD_MggZHJj0Zb2_1t8p95oqoFp_SVOlxflvBnepA3iD5vFT6Ai6DXB0SjuqZ337maKwHXwu0mv71e3_XNHoGygkrcIWQCer6qeKNgxBwF3tjXa4DJMOcQm-e4PQ',
    excerpt: 'Cliffside villas and azure waters await in this Mediterranean paradise.',
    description:
      'Wind along coastal roads past lemon groves and pastel villages. Private boat charters, seaside dining, and boutique stays curated for slow luxury.',
    highlights: ['Private yacht sunset', 'Villa sommelier dinner', 'Path of the Gods hike'],
    tags: ['COASTAL', 'EUROPE'],
  },
  {
    id: 'tokyo',
    name: 'Tokyo, Japan',
    region: 'Asia',
    category: 'Urban',
    priceLevel: 3,
    priceFrom: 310,
    image:
      'https://lh3.googleusercontent.com/aida-public/AB6AXuDMVPZyQmPXZnx0PYtp9I8Q54m5ROPjfQ0JfNsiZk1mbK9AN9Y53k6zKlVF-tSj6Cl9Wjb1JbQZfbZep7U5Z5K0h2r0D3lqLsCPoeaH01iCauiy2mysXS__FIAwLBd9xu9Vi2p0TYhBHEAhpxKUogWBhsy7baYglaDf8GSNv5Iyiwi5SSL6gK9hkIrCKPQxebVIPWX39ru9ZjCN_eW_9e3cdaJ9rP0f8-2Q3ku1H6xLw_qaZczVrCdPP-gwBFunKY8eS93bWaVTkDE',
    excerpt: 'Neon lights and serene temples.',
    description:
      'Contrast hyper-modern districts with quiet gardens and artisan neighborhoods. Insider access to chefs, galleries, and design-forward stays.',
    highlights: ['After-hours gallery walk', 'Omakase with sake pairing', 'Day trip to Kamakura'],
    tags: ['URBAN', 'ASIA'],
  },
  {
    id: 'zermatt',
    name: 'Zermatt, Switzerland',
    region: 'Europe',
    category: 'Alpine',
    priceLevel: 4,
    priceFrom: 580,
    image:
      'https://lh3.googleusercontent.com/aida-public/AB6AXuCIR4IjC5A9zYCeVpv-6E2Qsr-6XGwJt_c_ahEjpLc8ZwVjHtz_S8_ABYHEdi18syPgvEJC77vQYcnvKvAVrnJ-5bRklkm1OeYVO9jxHTy-VhSEjEjkfWAigDBqqn8KKjuyysokk89K1Z1lCxhl95xAyFVMl6o9qx6AXkGfZotuG2vff5RbIOgYiqc3ZFT-zAe5gggKvE6s6bvZ7OB012-vm0Lg_HGyIo-JXxaYx54YHQId_hs6eJfYg3MkrDG_aEW5LUR8x2juAyQ',
    excerpt: 'Pristine slopes and luxury chalets.',
    description:
      'Matterhorn views, glacier skiing, and fireside evenings. Chalet hosts, mountain guides, and wellness suites tailored to your pace.',
    highlights: ['Heli-glacier picnic', 'Private ski guide', 'Alpine spa ritual'],
    tags: ['ALPINE', 'EUROPE'],
  },
  {
    id: 'santorini',
    name: 'Santorini, Greece',
    region: 'Europe',
    category: 'Coastal',
    priceLevel: 3,
    priceFrom: 360,
    image:
      'https://lh3.googleusercontent.com/aida-public/AB6AXuBmuxz78CkUOTf5-2i-K-RA3SRKhZP0sQOPSNKZzrmyEwu8oLqJpQG6x6Mfym3dnnHxXx2g9TQkXA1Clvv-XI2lvoJXEdh8jaqGFhm5LuSC2Mb3gQxMxhCTyvj_Grfw6cxxBHdHV3YvV8Kln4kiivHHvHzcClnjwLBPWhaV5kFpGjhyjS6ULK8Wzpo6hqkTcvre-RpRnza8Xuh5q0n9Df6vgUzkdbo6U8ZSEOkurVGq5WPTb7Cps2vuto6xjjoWAf2OAfhfu254U-k',
    excerpt: 'Caldera views and volcanic wines.',
    description:
      'White-washed suites, sunset sails, and volcanic beaches. Curated tastings and island hopping away from the crowds.',
    highlights: ['Sunset catamaran', 'Winemaker cellar dinner', 'Akrotiri private tour'],
    tags: ['COASTAL', 'EUROPE'],
  },
];

export function getDestinationById(id) {
  return destinations.find((d) => d.id === id);
}
