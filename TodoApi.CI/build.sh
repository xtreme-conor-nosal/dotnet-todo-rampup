#!/bin/bash
set -e

cd TodoApi

dotnet restore
dotnet publish -c Release 

cd ..