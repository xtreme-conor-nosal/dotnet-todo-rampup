#!/bin/bash
set -e

cd TodoApi.SmokeTests

dotnet restore
dotnet xunit -c Release 

cd ..