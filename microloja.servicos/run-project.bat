@echo off
echo =====================================
echo    MICRO LOJA - PRODUTO API
echo =====================================
echo.

REM Verifica se o .NET 9 está instalado
echo [1/5] Verificando instalação do .NET...
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERRO: .NET não está instalado ou não está no PATH
    echo Baixe e instale o .NET 9 SDK em: https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

REM Exibe versão do .NET
for /f "tokens=*" %%i in ('dotnet --version') do set DOTNET_VERSION=%%i
echo .NET versão: %DOTNET_VERSION%
echo.

REM Navega para o diretório do projeto
echo [2/5] Navegando para o diretório do projeto...
cd /d "%~dp0MicroLoja.ProdutoAPI"

REM Restaura as dependências NuGet
echo [3/5] Restaurando dependências do projeto...
dotnet restore
if %errorlevel% neq 0 (
    echo ERRO: Falha ao restaurar dependências
    pause
    exit /b 1
)
echo Dependências restauradas com sucesso!
echo.

REM Compila o projeto
echo [4/5] Compilando o projeto...
dotnet build --configuration Debug --no-restore
if %errorlevel% neq 0 (
    echo ERRO: Falha na compilação do projeto
    pause
    exit /b 1
)
echo Projeto compilado com sucesso!
echo.

REM Executa o projeto
echo [5/5] Iniciando o servidor...
echo.
echo =====================================
echo   SERVIDOR INICIADO COM SUCESSO!
echo =====================================
echo.
echo URLs disponíveis:
echo - HTTPS: https://localhost:5001
echo - HTTP:  http://localhost:5000
echo - Swagger: https://localhost:5001 (abre automaticamente)
echo.
echo Pressione Ctrl+C para parar o servidor
echo.

REM Executa o projeto com configurações específicas
dotnet run --configuration Debug --verbosity minimal