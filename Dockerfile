# Use a imagem base do .NET SDK 8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Instala dependências necessárias para o Google Chrome e o ChromeDriver
RUN apt-get update && apt-get install -y wget unzip \
    && echo "Installed wget and unzip" \
    || { echo "Failed to install wget and unzip"; exit 1; }

RUN wget https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb -O /tmp/google-chrome.deb \
    && echo "Downloaded Google Chrome" \
    || { echo "Failed to download Google Chrome"; exit 1; }

RUN apt install -y /tmp/google-chrome.deb \
    && echo "Installed Google Chrome" \
    || { echo "Failed to install Google Chrome"; exit 1; }

# Verifica se a URL do ChromeDriver está acessível
RUN wget https://storage.googleapis.com/chrome-for-testing-public/129.0.6668.100/linux64/chromedriver-linux64.zip -O /tmp/chromedriver-linux64.zip \
    && echo "Downloaded ChromeDriver" \
    || { echo "Failed to download ChromeDriver"; exit 1; }

RUN unzip /tmp/chromedriver-linux64.zip -d /usr/local/bin/ \
    && echo "Unzipped ChromeDriver" \
    || { echo "Failed to unzip ChromeDriver"; exit 1; }

RUN chmod +x /usr/local/bin/chromedriver-linux64 \
    && echo "Made ChromeDriver executable" \
    || { echo "Failed to make ChromeDriver executable"; exit 1; }

# Limpeza
RUN rm -rf /var/lib/apt/lists/* /tmp/google-chrome.deb /tmp/chromedriver-linux64.zip \
    && echo "Cleaned up temporary files" \
    || { echo "Failed to clean up temporary files"; exit 1; }

# Defina o diretório de trabalho
WORKDIR /app

COPY MeuClienteWebTestProject.csproj ./
RUN dotnet restore

# Copia o restante dos arquivos e compila
COPY . ./
RUN dotnet build --configuration Release -o out

# Define o comando padrão
CMD ["dotnet", "test", "MeuClienteWebTestProject.csproj"]