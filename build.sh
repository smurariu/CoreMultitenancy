#!/bin/bash
dotnet clean && dotnet pack src/WebApi.Multitenancy/ && dotnet build