#!/bin/bash
Platforms=('android-arm64' 'linux-arm64' 'win-x64' 'linux-x64' 'osx-x64')

if ! [ -d './TempFiles' ];
then
    mkdir ./TempFiles
fi

rm -rf ./TempFiles/*

for platform in "${Platforms[@]}"
do
    dotnet publish -r "$platform" -c Release -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true || exit 1

    cd ./bin/Release/net6.0/"$platform"/publish/ || exit 1
    zip -r ./ZonyLrcTools_"$platform"_"${PUBLISH_VERSION}".zip ./ || exit 1
    cd ../../../../../ || exit 1

    mv ./bin/Release/net6.0/"$platform"/publish/ZonyLrcTools_"$platform"_"$PUBLISH_VERSION".zip ./TempFiles
done
