# CryptoWallet 
__Небольной проект криптокошелька с блокчейн технологией на .NET Core__

В ходе разработки были использованы сторонние библиотеки:
- __NBitCoin 6.0.18__ https://www.nuget.org/packages/NBitcoin/ - Использовалась только для правильной генерации ключей по алгоритму Secp256k1
- __Newtonsoft.Json 13.0.1__ https://www.nuget.org/packages/Newtonsoft.Json - Использовалась, как более производительная альтернатива встроенной системной библиотеки __System.Text.Json__

## Установка библиотек:
``` 
dotnet add package NBitcoin --version 6.0.18
dotnet add package Newtonsoft.Json --version 13.0.1
```

## Запуск
1. Склонировать репозиторий
    ``` git
    git clone https://github.com/Amonlol/CryptoWallet
    ```
2. Установить внешние библиотеки
    ``` git
    dotnet add package NBitcoin --version 6.0.18
    dotnet add package Newtonsoft.Json --version 13.0.1
    ```
3. Открыть с помощью Visual Studio и скомпилировать проект
4. Запустить сервер Server/bin/Debug/Server.exe (дебаг) либо Server/bin/Release/Server.exe (релиз)
5. Запустить клиентское приложение ClientUI/bin/Debug/ClientUI.exe (дебаг) либо ClientUI/bin/Release/ClientUI.exe (релиз)

## Структура папок:
```
│   .gitignore
│   CryptoWallet.sln <-- Файл проекта
│   ReadMe.md <-- Файл readme проекта
│               
├───ClientUI <-- Папка клиентского приложения на WinForms
│   │   App.config
│   │   AuthenticationForm.cs <-- форма для авторизации клиента
│   │   AuthenticationForm.Designer.cs
│   │   AuthenticationForm.resx
│   │   ClientUI.csproj
│   │   ClientUI.csproj.user
│   │   ClientUI_TemporaryKey.pfx
│   │   Form1.cs <-- Форма логики всего клиентского приложения
│   │   Form1.Designer.cs
│   │   Form1.resx
│   │   LoggedForm.cs <-- Форма после успешной авторизации клиента
│   │   LoggedForm.Designer.cs
│   │   LoggedForm.resx
│   │   packages.config
│   │   Program.cs <-- Основная точка входа в клиентское приложение
│   │   RegisterForm.cs <-- Форма регистрации нового клиента
│   │   RegisterForm.Designer.cs
│   │   RegisterForm.resx
│   │   
│   └───bin
│       ├───Debug
│       │       ClientUI.exe <-- Запуск дебаг-версии клиентского приложения
│       │       
│       └───Release
│               ClientUI.exe <-- Запуск релиз-версии клиентского приложения
│
├───CryptoLibrary <-- Папка самописной библиотеки
│   │   App.config
│   │   CryptoLibrary.csproj
│   │   CryptoLibrary.csproj.user
│   │   packages.config
│   │   
│   ├───BlockChain <-- Папка с описанием блокчейна
│   │       Block.cs
│   │       Chain.cs
│   │       
│   ├───DataBase <-- TODO: добавить возможность хранения данных в базе данных
│   │       ChainDB.cs
│   │       
│   ├───Network <-- Папка с описанием межсетевого взаимодействия
│   │       Network.cs
│   │     
│   ├───Transaction <-- Папка для генерации новой транзакции
│   │       Transaction.cs
│   │       
│   ├───User <-- Папка с описанием пользователя
│   │       Address.cs
│   │       
│   └───Utility <-- Папка с служебными методами
│           UtilityClass.cs
│           
└───Server <-- Папка с серверным приложением
    │   App.config
    │   packages.config
    │   Program.cs <-- Точка входа в серверное приложение
    │   Server.csproj
    │   Server.csproj.user
    │   Server_TemporaryKey.pfx
    │   
    └───bin
        ├───Debug
        │       Server.exe <-- Запуск дебаг-версии серверного приложения
        │       
        └───Release
                Server.exe <-- Запуск релиз-версии серверного приложения
                
```