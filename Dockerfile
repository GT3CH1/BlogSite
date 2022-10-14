FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
ADD ["BlogSite/BlogSite.csproj", "BlogSite/"]
RUN dotnet restore "BlogSite/BlogSite.csproj"
COPY . .
WORKDIR "/src/BlogSite"
RUN dotnet build "BlogSite.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlogSite.csproj" -c Release -o /app/publish
#RUN apt install wget
#RUN wget https://dot.net/v1/dotnet-install.sh
#RUN chmod +x ./dotnet-install.sh ; ./dotnet-install.sh -c Current
FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "--environment","production","BlogSite.dll"]
