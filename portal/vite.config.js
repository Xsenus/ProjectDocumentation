import { defineConfig } from 'vite';

const backend = { target: 'http://127.0.0.1:8765', changeOrigin: true };

export default defineConfig({
  base: './',
  esbuild: { jsx: 'automatic' },
  server: {
    host: '127.0.0.1',
    proxy: {
      '/api': backend,
      '/schemas': backend,
    },
  },
});
