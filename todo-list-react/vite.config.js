import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
    plugins: [react()],
    server: {
        open: true,  // Это заставит Vite открыть браузер автоматически
        //port: 5173   // Если нужно фиксировать порт (по умолчанию 5173)
        proxy: {
            '/api': {
                target: 'https://localhost:7094',  // Замените на порт вашего ASP.NET (смотрите в launchSettings.json или в консоли при запуске)
                changeOrigin: true,
                secure: false
            }
        }
    },
    
})
