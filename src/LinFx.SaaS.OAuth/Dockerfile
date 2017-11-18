FROM microsoft/aspnetcore
ARG source
WORKDIR /app
EXPOSE 80
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "LinFx.SaaS.OAuth.dll"]
