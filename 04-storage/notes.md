## Storage Account Notes

- Used Standard performance with LRS for cost efficiency
- Containers separated by data sensitivity
- Public access limited to blob-level only
- Private data accessed securely via SAS tokens
- SAS configured with:
  - Read-only permission
  - HTTPS enforced
  - Short expiration time

## Security considerations
- Access Keys provide full control and should be avoided for user access
- SAS is preferred for temporary or scoped access
- Network restrictions reduce exposure surface

## AZ-104 relevance
- Storage account configuration
- Blob access levels
- SAS usage
- Storage security best practices
