cd ..\Zuul.Web\packages\FluentMigrator.Tools.1.6.2\tools\AnyCPU\40
Migrate.exe /connection "Server=localhost\SQLExpress;User Id=VigoUser;Password=abc@1234;initial catalog=ZuulDB;Integrated Security=True" /db Sqlserver2008 /timeout 600 /target ..\..\..\..\..\..\Zuul.Migrations\bin\Debug\Zuul.Migrations.dll
cd C:\Work\Zuul\utils