# trip-assistant-api

## Description

Record and calculate expenses for trip members.

## Table of Contents

- [Features](#features)
- [Installation](#installation)
- [Configuration](#configuration)


## Features
- record trip, trip members, and expense transactions
- calculate expense for trip members


## Installation

### Prerequisites

- .NET8
- Mysql 9.1
- Cognito user pool

### Build the project:

```
dotnet restore
```

### Start the project:

```
dotnet run
```


## Configuration
environment variables should to update.
- ASPNETCORE_ENVIRONMENT
- Jwt__SecretKey 
- Jwt__Issuer 
- Jwt__Audience 
- ConnectionStrings__TripAssistantConnectionString
- Aws__CognitoRegion
- Aws__CognitoUserPoolId
- AWS__CognitoUserInfoUrl

