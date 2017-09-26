#!/bin/bash
set -e

cd TodoApi/TodoApi.Tests

dotnet restore
dotnet xunit