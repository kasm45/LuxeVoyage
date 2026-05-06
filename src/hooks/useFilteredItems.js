import { useMemo } from 'react';

/**
 * @param {Array} items — must have category, region, and priceFrom (number)
 */
export function useFilteredItems(items, { category, region, maxPrice }) {
  return useMemo(() => {
    return items.filter((item) => {
      if (category && category !== 'All' && item.category !== category) return false;
      if (region && region !== 'All' && item.region !== region) return false;
      if (maxPrice != null && item.priceFrom > maxPrice) return false;
      return true;
    });
  }, [items, category, region, maxPrice]);
}
