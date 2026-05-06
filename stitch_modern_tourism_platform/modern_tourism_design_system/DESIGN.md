---
name: Modern Tourism Design System
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
  on-surface-variant: '#45464d'
  inverse-surface: '#2d3133'
  inverse-on-surface: '#eff1f3'
  outline: '#76777d'
  outline-variant: '#c6c6cd'
  surface-tint: '#565e74'
  primary: '#000000'
  on-primary: '#ffffff'
  primary-container: '#131b2e'
  on-primary-container: '#7c839b'
  inverse-primary: '#bec6e0'
  secondary: '#855300'
  on-secondary: '#ffffff'
  secondary-container: '#fea619'
  on-secondary-container: '#684000'
  tertiary: '#000000'
  on-tertiary: '#ffffff'
  tertiary-container: '#00201d'
  on-tertiary-container: '#0c9488'
  error: '#ba1a1a'
  on-error: '#ffffff'
  error-container: '#ffdad6'
  on-error-container: '#93000a'
  primary-fixed: '#dae2fd'
  primary-fixed-dim: '#bec6e0'
  on-primary-fixed: '#131b2e'
  on-primary-fixed-variant: '#3f465c'
  secondary-fixed: '#ffddb8'
  secondary-fixed-dim: '#ffb95f'
  on-secondary-fixed: '#2a1700'
  on-secondary-fixed-variant: '#653e00'
  tertiary-fixed: '#89f5e7'
  tertiary-fixed-dim: '#6bd8cb'
  on-tertiary-fixed: '#00201d'
  on-tertiary-fixed-variant: '#005049'
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
    fontFamily: Plus Jakarta Sans
    fontSize: 24px
    fontWeight: '600'
    lineHeight: '1.4'
  body-lg:
    fontFamily: Plus Jakarta Sans
    fontSize: 18px
    fontWeight: '400'
    lineHeight: '1.6'
  body-md:
    fontFamily: Plus Jakarta Sans
    fontSize: 16px
    fontWeight: '400'
    lineHeight: '1.6'
  label-sm:
    fontFamily: Plus Jakarta Sans
    fontSize: 14px
    fontWeight: '600'
    lineHeight: '1.0'
    letterSpacing: 0.05em
rounded:
  sm: 0.25rem
  DEFAULT: 0.5rem
  md: 0.75rem
  lg: 1rem
  xl: 1.5rem
  full: 9999px
spacing:
  base: 8px
  xs: 4px
  sm: 12px
  md: 24px
  lg: 48px
  xl: 80px
  gutter: 24px
  margin-mobile: 16px
  margin-desktop: 64px
---

## Brand & Style

This design system is built to evoke a sense of professional wanderlust and effortless discovery. The brand personality is **sophisticated, adventurous, and reliable**, targeting travelers who value both inspiration and clarity. 

The aesthetic follows a **Modern/Minimalist** approach with subtle **Glassmorphism** influences. It prioritizes high-quality imagery by using generous whitespace and crisp typography to frame content without competing with it. The UI feels airy and premium, utilizing translucent layers for navigation and overlays to maintain a connection with the background visuals.

## Colors

The palette is anchored by **Deep Navy** to establish authority and depth. This is balanced by **Crisp Whites** and **Slate Grays** to ensure a clean, modern foundation. 

**Vibrant Accents** are used strategically: 
- **Sunset Orange** is reserved for primary Calls to Action (CTAs) and urgency-based indicators (e.g., "Only 2 spots left").
- **Teal** serves as a secondary accent for success states, tags, and interactive elements related to nature or water destinations.
- **Deep Navy** is used for primary text and grounding elements like footers and headers.

## Typography

The typography strategy employs a high-contrast pairing to balance editorial elegance with functional readability.

**Noto Serif** is used for primary headlines (H1, H2) to provide a literary, premium feel reminiscent of high-end travel magazines. **Plus Jakarta Sans** is the workhorse for all UI elements, body copy, and navigation, offering a friendly and approachable geometric style. 

- Use **All-caps** for labels and overlines to improve hierarchy.
- Maintain generous **line-height** (1.6) for body text to ensure readability during long-form destination guides.

## Layout & Spacing

The design system utilizes a **12-column fluid grid** for the public-facing site, allowing for flexible content blocks and immersive imagery. 

### Public-Facing Layout
- Focus on large vertical spacing (`xl`) between sections to create a sense of breathability.
- Content containers should have a maximum width of 1280px.
- Use asymmetrical layouts for image galleries to create visual interest.

### Admin Panel Layout
- **Sidebar-based:** A fixed 280px left-hand sidebar for navigation.
- **Content Area:** A fluid canvas with `md` padding on all sides.
- Use a 4-column sub-grid within the admin content area for dashboard widgets.

## Elevation & Depth

This design system uses **Tonal Layers** and **Ambient Shadows** to create a structured hierarchy.

- **Level 0 (Base):** Neutral background (#F8FAFC).
- **Level 1 (Cards/Sidebar):** Pure white surface with a 1px border (#E2E8F0) and no shadow for a flat, clean look.
- **Level 2 (Dropdowns/Hover States):** Soft, diffused shadow (0px 10px 15px -3px rgba(15, 23, 42, 0.08)).
- **Level 3 (Modals/Overlays):** Heavy blur with a backdrop filter (Blur: 12px, Opacity: 80%) to create the glassmorphism effect, ensuring the user remains grounded in the current context.

## Shapes

The shape language is **Rounded (Level 2)**, conveying friendliness and modern comfort. 

- **Standard Elements:** 8px (0.5rem) corner radius for buttons and input fields.
- **Large Elements:** 16px (1rem) for cards and main containers.
- **Specialty Elements:** 24px (1.5rem) for "Featured" callouts or promotional banners to make them feel softer and more distinct.

## Components

### Buttons
- **Primary:** Deep Navy background with White text. Bold and authoritative.
- **Secondary:** Transparent with Sunset Orange border and text for secondary actions.
- **CTA:** Sunset Orange background for high-conversion booking buttons.

### Cards
- **Destination Cards:** Edge-to-edge imagery at the top, followed by 16px padding for content. Use a subtle 1px border instead of heavy shadows.
- **Price Tags:** Small floating chips in the top-right corner using the Teal accent background.

### Form Elements
- **Inputs:** Simple 1px border with a 48px height. On focus, the border transitions to Deep Navy with a 2px stroke.
- **Search Bar:** A specialized component for the public site featuring a "Glass" background (semi-transparent white) when overlaying hero images.

### Admin Sidebar
- **Active State:** A vertical Sunset Orange bar (4px wide) on the left edge of the active menu item.
- **Icons:** Thin-stroke (2pt) icons to match the elegant typography.

### Chips & Tags
- Used for categories (e.g., "Beach," "Adventure," "Luxury"). These should be low-contrast (Light Gray background) to avoid distracting from primary imagery.