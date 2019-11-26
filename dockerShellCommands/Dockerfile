FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["dockerShellCommands/dockerShellCommands.csproj", "dockerShellCommands/"]
RUN dotnet restore "dockerShellCommands/dockerShellCommands.csproj"
COPY . .
WORKDIR "/src/dockerShellCommands"
RUN dotnet build "dockerShellCommands.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "dockerShellCommands.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "dockerShellCommands.dll"]