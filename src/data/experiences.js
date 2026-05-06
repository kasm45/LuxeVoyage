export const experiences = [
  {
    id: 'private-vineyard',
    title: 'Private Vineyard Tour',
    region: 'Europe',
    category: 'Food & Wine',
    priceFrom: 450,
    icon: 'wine_bar',
    image:
      'https://lh3.googleusercontent.com/aida-public/AB6AXuD5Es1adMGCAvCmj1RVpGBoPpoF4UduxQOJ6wtmLgnD8wTPVSfOS_TGUxYHFst3LSrTS0CCNt1TTMCCFc3F3RY9Zs783o_MGJyg7qRa6xixyBrr6zR8nEG7nEhYY3BbRAu8xLfrOYUIuUk0e0oVBBksUL1hdmBRr4n3R-24W6HnRS4PqHU8QypZMn4WdvDVoXqnHKjzU3956vX0JOL8AG3ij24tVPlXqmfHahcpQUuh2Q0d4yTehJ8zysZnLLQ3xUGvw9RWD_q-Kao',
    excerpt: 'Exclusive tastings in hidden cellars across Tuscany.',
    description:
      'Meet vintners, walk ancient rows, and savor limited releases in candlelit cellars. Transportation and lunch included.',
    duration: '6 hours',
  },
  {
    id: 'aerial-canyon',
    title: 'Aerial Canyon Tour',
    region: 'Americas',
    category: 'Adventure',
    priceFrom: 890,
    icon: 'flight_takeoff',
    image:
      'https://lh3.googleusercontent.com/aida-public/AB6AXuBfLmPPvRhBGcmEobeHhHO2a_7h3knpyF4eiuW5aZdkCyYcsB0Z-qb17FEXLnEeYuUqBeH0vl690tP127JT9xggNqhPlFLMHU0ClCvTtSBIkduweCdWpBHiq63KiXPP2ul1psaTeQQR-U4alttTY_BS2h_ek1d_EGIzLc0awsNarTDMvXgqcMcgogxod07JTOuubpA3042YuMsHetKdqjEy8HYCw1lwhS_SdnXrYrPwaYFQpjcjbtYFjy5TWRpp4kRh3AVKgUObTRY',
    excerpt: 'Soar above majestic landscapes in a luxury chopper.',
    description:
      'Door-off optional routes with certified pilots, champagne landing, and photography briefing.',
    duration: '90 minutes',
  },
  {
    id: 'holistic-wellness',
    title: 'Holistic Wellness',
    region: 'Asia',
    category: 'Wellness',
    priceFrom: 300,
    icon: 'spa',
    image:
      'https://lh3.googleusercontent.com/aida-public/AB6AXuAtlwdsFDJy6Z5yCktCcIz6jG6FvpVgM9u-6tuFV0-Wn5wbXiyykkkb3qWv4pkpIguFDLYgJwkYr0sAgQZYn3mFRDBOgX2V3GRk4e0rBgsfNqaf_J7XV8jGRpVa9yYW418Nlkjbb9-zDtoCHZzMOG-28kyPmYU-Q_O2eTfsTEE8vkb8Km_Fs0f8CP73K6r6Lyf2ef5EnicNomRDQ-BLDtfh0JCLUSQboaINkhi5GI0Cs6RLW9BEw-47at0SkZeJy8lq2fSocbSDNUQ',
    excerpt: 'Rejuvenate with ancient practices in modern luxury.',
    description:
      'Personal ayurvedic consult, thermal circuits, and guided breathwork in a cliffside sanctuary.',
    duration: 'Half day',
  },
  {
    id: 'blue-lagoon-private',
    title: 'Blue Lagoon Private Suite',
    region: 'Europe',
    category: 'Wellness',
    priceFrom: 520,
    icon: 'water_drop',
    image:
      'https://lh3.googleusercontent.com/aida-public/AB6AXuBmuxz78CkUOTf5-2i-K-RA3SRKhZP0sQOPSNKZzrmyEwu8oLqJpQG6x6Mfym3dnnHxXx2g9TQkXA1Clvv-XI2lvoJXEdh8jaqGFhm5LuSC2Mb3gQxMxhCTyvj_Grfw6cxxBHdHV3YvV8Kln4kiivHHvHzcClnjwLBPWhaV5kFpGjhyjS6ULK8Wzpo6hqkTcvre-RpRnza8Xuh5q0n9Df6vgUzkdbo6U8ZSEOkurVGq5WPTb7Cps2vuto6xjjoWAf2OAfhfu254U-k',
    excerpt: 'Geothermal rituals with a dedicated host.',
    description:
      'Reserved lagoon access, in-water massage, and chef-driven tasting featuring Nordic ingredients.',
    duration: '4 hours',
  },
];

export function getExperienceById(id) {
  return experiences.find((e) => e.id === id);
}
