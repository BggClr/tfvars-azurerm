# tfvars

### Using

```bash
tfvars init --a account --c container
tfvars project list
tfvars project add --project-id projectId
tfvars secrets get --project-id projectId
tfvars secrets get --project-id projectId --init
tfvars secrets upload --project-id projectId [--filename terraform.tfvars]
```

### Publish

```bash
dotnet publish -c Release -r win-x64 -o ./out
dotnet publish -c Release -r osx-arm64 -o ./out
```

#### *Requirements

User (or one of the Azure Entra ID) should have **Storage Blob Data Contributor** role at Storage Account.
