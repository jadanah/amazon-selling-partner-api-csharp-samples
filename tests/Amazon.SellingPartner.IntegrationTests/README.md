
```sh
dotnet user-secrets init
```

```sh
dotnet user-secrets set "Amzn:AWSKey" "AWS_KEY"
dotnet user-secrets set "Amzn:AWSSecret" "AWS_SECRET"
dotnet user-secrets set "Amzn:ClientId" "amzn1.application-oa2-client.xyz"
dotnet user-secrets set "Amzn:ClientSecret" "CLIENT_SECRET"
dotnet user-secrets set "Amzn:RefreshToken" "REFRESH_TOKEN"
dotnet user-secrets set "Amzn:RoleARN" "arn:aws:iam::1234:role/ROLE_NAME"
```