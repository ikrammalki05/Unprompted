# ── Build stage ────────────────────────────────────────────────────────────
FROM node:20-alpine AS build

WORKDIR /app

# Install dependencies first (layer cache — only re-runs if package.json changes)
COPY package.json package-lock.json ./
RUN npm ci

# Copy source and build
COPY . .

ARG VITE_API_URL=http://localhost:5000
ENV VITE_API_URL=$VITE_API_URL

RUN npm run build

# ── Runtime stage (nginx to serve static files) ─────────────────────────────
FROM nginx:1.27-alpine AS runtime

# Copy built assets
COPY --from=build /app/dist /usr/share/nginx/html

# Custom nginx config: route all requests to index.html (React Router SPA)
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]