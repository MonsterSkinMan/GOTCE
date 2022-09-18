rm -rf ./GOTCE/obj/Debug
dotnet restore
dotnet build
rm -rf ./GOTCE/C:
cp -r ./GOTCE/obj/Debug/ ~/.config/r2modmanPlus-local/RiskOfRain2/profiles/testing/BepInEx/plugins/