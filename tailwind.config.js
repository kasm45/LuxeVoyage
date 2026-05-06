/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{js,jsx}'],
  darkMode: 'class',
  theme: {
    extend: {
      colors: {
        background: '#f7f9fb',
        surface: '#ffffff',
        'on-background': '#191c1e',
        'on-surface': '#191c1e',
        'on-surface-variant': '#584237',
        primary: '#9d4300',
        'primary-container': '#f97316',
        'on-primary': '#ffffff',
        'on-primary-container': '#582200',
        'surface-tint': '#9d4300',
        secondary: '#565e74',
        'secondary-container': '#dae2fd',
        'on-secondary-container': '#5c647a',
        tertiary: '#006a66',
        'surface-variant': '#e0e3e5',
        'surface-container-low': '#f2f4f6',
        outline: '#8c7164',
      },
      fontFamily: {
        serif: ['"Noto Serif"', 'Georgia', 'serif'],
        sans: ['Inter', 'system-ui', 'sans-serif'],
      },
      fontSize: {
        h1: ['2.5rem', { lineHeight: '1.2', letterSpacing: '-0.02em', fontWeight: '700' }],
        h2: ['2rem', { lineHeight: '1.25', fontWeight: '600' }],
        h3: ['1.5rem', { lineHeight: '1.35', fontWeight: '600' }],
        'body-lg': ['1.125rem', { lineHeight: '1.6' }],
        'label-caps': ['0.75rem', { lineHeight: '1', letterSpacing: '0.05em', fontWeight: '600' }],
      },
      boxShadow: {
        card: '0 4px 20px rgba(15, 23, 42, 0.05)',
        cardHover: '0 12px 40px rgba(15, 23, 42, 0.08)',
      },
      maxWidth: {
        container: '1280px',
      },
      spacing: {
        gutter: '24px',
        xxl: '80px',
      },
    },
  },
  plugins: [],
};
