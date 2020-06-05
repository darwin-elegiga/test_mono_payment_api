find . \( -name "*.csproj" -o -name "*.sln" -o -iname "nuget.config" \) -print0 | tar -cvf projectfiles.tar --null -T -
