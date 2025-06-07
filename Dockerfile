FROM mcr.microsoft.com/dotnet/sdk:9.0

WORKDIR /app

COPY ./publish_output .

ENTRYPOINT ["dotnet", "CRMBackend.Web.dll"]