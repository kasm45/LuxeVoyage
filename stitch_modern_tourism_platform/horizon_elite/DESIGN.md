---
name: Horizon Elite
colors:
  surface: '#f7f9fb'
  surface-dim: '#d8dadc'
  surface-bright: '#f7f9fb'
  surface-container-lowest: '#ffffff'
  surface-container-low: '#f2f4f6'
  surface-container: '#eceef0'
  surface-container-high: '#e6e8ea'
  surface-container-highest: '#e0e3e5'
  on-surface: '#191c1e'
  on-surface-variant: '#584237'
  inverse-surface: '#2d3133'
  inverse-on-surface: '#eff1f3'
  outline: '#8c7164'
  outline-variant: '#e0c0b1'
  surface-tint: '#9d4300'
  primary: '#9d4300'
  on-primary: '#ffffff'
  primary-container: '#f97316'
  on-primary-container: '#582200'
  inverse-primary: '#ffb690'
  secondary: '#565e74'
  on-secondary: '#ffffff'
  secondary-container: '#dae2fd'
  on-secondary-container: '#5c647a'
  tertiary: '#006a66'
  on-tertiary: '#ffffff'
  tertiary-container: '#2eaba5'
  on-tertiary-container: '#003937'
  error: '#ba1a1a'
  on-error: '#ffffff'
  error-container: '#ffdad6'
  on-error-container: '#93000a'
  primary-fixed: '#ffdbca'
  primary-fixed-dim: '#ffb690'
  on-primary-fixed: '#341100'
  on-primary-fixed-variant: '#783200'
  secondary-fixed: '#dae2fd'
  secondary-fixed-dim: '#bec6e0'
  on-secondary-fixed: '#131b2e'
  on-secondary-fixed-variant: '#3f465c'
  tertiary-fixed: '#84f5ee'
  tertiary-fixed-dim: '#66d8d2'
  on-tertiary-fixed: '#00201e'
  on-tertiary-fixed-variant: '#00504d'
  background: '#f7f9fb'
  on-background: '#191c1e'
  surface-variant: '#e0e3e5'
typography:
  h1:
    fontFamily: Noto Serif
    fontSize: 48px
    fontWeight: '700'
    lineHeight: '1.2'
    letterSpacing: -0.02em
  h2:
    fontFamily: Noto Serif
    fontSize: 36px
    fontWeight: '600'
    lineHeight: '1.3'
  h3:
    fontFamily: Noto Serif
    fontSize: 24px
    fontWeight: '600'
    lineHeight: '1.4'
  body-lg:
    fontFamily: Inter
    fontSize: 18px
    fontWeight: '400'
    lineHeight: '1.6'
  body-md:
    fontFamily: Inter
    fontSize: 16px
    fontWeight: '400'
    lineHeight: '1.5'
  label-caps:
    fontFamily: Inter
    fontSize: 12px
    fontWeight: '600'
    lineHeight: '1.0'
    letterSpacing: 0.05em
  button:
    fontFamily: Inter
    fontSize: 16px
    fontWeight: '600'
    lineHeight: '1.0'
rounded:
  sm: 0.25rem
  DEFAULT: 0.5rem
  md: 0.75rem
  lg: 1rem
  xl: 1.5rem
  full: 9999px
spacing:
  unit: 4px
  xs: 4px
  sm: 8px
  md: 16px
  lg: 24px
  xl: 48px
  xxl: 80px
  container-max: 1280px
  gutter: 24px
---

## Brand & Style
The design system is anchored in the concept of "Refined Exploration." It balances the rugged spirit of adventure with the seamless polish of high-end hospitality. The target audience consists of discerning travelers who value both inspiration and reliability.

The visual style is **Modern Corporate with Glassmorphism accents**. It utilizes heavy whitespace to evoke a sense of calm and luxury, reminiscent of an editorial travel magazine. High-quality, full-bleed imagery serves as the primary emotional driver, while UI elements remain secondary and unobtrusive. Translucent "glass" overlays are used for search bars and information cards over imagery to maintain context without sacrificing legibility.

## Colors
The palette is designed to be high-contrast yet sophisticated. 
- **Sunset Orange (#F97316):** Used exclusively for primary calls-to-action and key interactive highlights to drive conversion.
- **Deep Travel Blue (#0F172A):** The foundation for typography, navigation bars, and footer backgrounds, providing a grounded, trustworthy feel.
- **Cloud White (#F8FAFC):** The primary canvas color, ensuring the interface feels airy and spacious.
- **Subtle Gradients:** Use a linear gradient from #F97316 to #FB923C for primary buttons to add depth and a "sunlight" glow.

## Typography
This design system employs a tiered typographic strategy. **Noto Serif** is reserved for storytelling elements, such as destination titles and editorial headlines, creating a sense of timeless luxury. **Inter** handles all functional data, navigation, and body copy to ensure maximum legibility and a modern, app-like feel. Use tight tracking on serif headlines for a premium look, and increased line height on body text to enhance readability.

## Layout & Spacing
The layout follows a **12-column fixed grid** system for desktop, transitioning to a fluid single column for mobile. Spaciousness is a core principle; use the `xxl` (80px) spacing unit between major content sections to allow the user's eyes to rest. 

- **Margins:** 24px on mobile, 48px or auto-centered on desktop.
- **Rhythm:** All vertical spacing should be a multiple of the 4px base unit. 
- **Content Density:** Maintain low density for discovery pages and medium density for booking/checkout flows.

## Elevation & Depth
Hierarchy is established through a mix of ambient shadows and glassmorphism:
- **Level 1 (Base):** Flat on Cloud White.
- **Level 2 (Cards):** Very soft, diffused shadow (0px 4px 20px rgba(15, 23, 42, 0.05)) to lift content subtly.
- **Level 3 (Overlays/Floating):** Background blur (20px) with a semi-transparent white fill (rgba(255, 255, 255, 0.7)) and a 1px white border at 20% opacity. This "Glass" effect is used for search bars and sticky navigation to maintain a sense of depth and context over high-resolution imagery.

## Shapes
The shape language is consistently "Rounded" to evoke friendliness and modern comfort. 
- **Standard Elements:** Use a 16px (1rem) radius for cards, image containers, and large buttons.
- **Small Elements:** Use 8px (0.5rem) for input fields and chips.
- **Interactive States:** When hovered, cards may slightly increase in scale (1.02x) while maintaining their radius, creating a fluid, tactile response.

## Components
- **Buttons:** Primary buttons use the Sunset Orange gradient with 16px corners and Inter Semi-Bold. Secondary buttons use a Deep Travel Blue outline or a ghost style.
- **Cards:** Travel destination cards feature high-quality imagery with a subtle bottom-to-top dark gradient overlay to ensure white text legibility. 16px corner radius is mandatory.
- **Inputs:** Search inputs should be large, using the Glassmorphism style when placed over hero images. Focus states are indicated by a 2px Sunset Orange border.
- **Chips/Tags:** Used for "Amenities" or "Categories." These use a light tint of Deep Travel Blue (5% opacity) with 40px (pill) roundedness for a soft, secondary feel.
- **Navigation:** A clean top-bar with Deep Travel Blue text. Active states are marked by a small Sunset Orange dot beneath the label rather than an underline.
- **Imagery:** All photos must have a slight 0.5px inner border in white (10% opacity) to ensure they pop against both light and dark backgrounds.