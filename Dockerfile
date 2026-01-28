# Base oficial do .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0

# Instalar dependências do Chrome
RUN apt-get update && apt-get install -y wget unzip curl gnupg \
    libxss1 fonts-liberation xdg-utils libnss3 libatk-bridge2.0-0 libcups2 \
    libxcomposite1 libxrandr2 libgbm1

# Instalar Google Chrome
RUN wget -q https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb \
    && apt install -y ./google-chrome-stable_current_amd64.deb

# Instalar ChromeDriver compatível
RUN CHROME_VERSION=$(google-chrome --version | grep -oP '\d+' | head -1) \
    && DRIVER_VERSION=$(curl -s "https://googlechromelabs.github.io/chrome-for-testing/LATEST_RELEASE_${CHROME_VERSION}") \
    && wget -q https://storage.googleapis.com/chrome-for-testing-public/${DRIVER_VERSION}/linux64/chromedriver-linux64.zip \
    && unzip chromedriver-linux64.zip \
    && mv chromedriver-linux64/chromedriver /usr/bin/chromedriver \
    && chmod +x /usr/bin/chromedriver

# Validar instalação
RUN google-chrome --version && chromedriver --version

WORKDIR /app