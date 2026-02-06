/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        dominos: {
          blue: '#0066CC',
          'blue-dark': '#004C99',
          red: '#E31837',
          'red-dark': '#C41230',
        },
        badge: {
          new: '#0066CC',
          vege: '#00A651',
          hot: '#FF6600',
          bestseller: '#FFD700',
        }
      },
      fontFamily: {
        sans: ['Inter', 'system-ui', 'sans-serif'],
      },
    },
  },
  plugins: [],
}
