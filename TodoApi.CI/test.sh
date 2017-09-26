#!/bin/bash
set -e

cd TodoApi.Tests

dotnet restore
dotnet xunit

cd ..