FROM mcr.microsoft.com/dotnet/aspnet:6.0

RUN mkdir /app
WORKDIR /app

COPY ./publish ./

ENTRYPOINT ["dotnet", "GGroupp.Platrom.Bot.ProactiveMessage.Send.Application.dll"]