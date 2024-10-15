# Use a imagem base do .NET SDK 8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Instala dependências necessárias para o Google Chrome e o ChromeDriver
RUN apt-get update && apt-get install -y wget unzip \
    && wget -q https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb -O /tmp/google-chrome.deb \
    && apt install -y /tmp/google-chrome.deb \
    && wget -q https://chromedriver.storage.googleapis.com/129.0.6668.100/chromedriver_linux64.zip -O /tmp/chromedriver.zip \
    && unzip /tmp/chromedriver.zip -d /usr/local/bin/ \
    && chmod +x /usr/local/bin/chromedriver \
    && rm -rf /var/lib/apt/lists/* /tmp/google-chrome.deb /tmp/chromedriver.zip

# Defina o diretório de trabalho
WORKDIR /app

# Comando de entrada padrão (opcional, você pode mudar dependendo do que deseja executar)
CMD ["dotnet", "--version"]