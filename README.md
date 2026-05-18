# RemovePlayerNameFromClaim

Server-side utility mod that hides Vintage Story land claim owner names from clients.

`RemovePlayerNameFromClaim` is a lightweight server-side mod for Vintage Story 1.22.x.

It replaces land claim owner names sent to clients with `Unknown`, helping PvP and survival servers avoid leaking free intelligence about who owns a protected area.

The mod is intentionally narrow:

- Server-side only
- Clients do not need to install it
- It does not modify saved claim ownership
- It only changes what clients see in claim sync packets and claim-related no-permission messages

Real ownership, permissions and admin commands remain unchanged on the server.

## Usage

1. Install the mod zip on the server.
2. Restart the server.
3. Existing and new claims will still work normally, but owner names shown to clients will be replaced with `Unknown`.

## Changelog 1.0.0

- Initial release
- Added server-side claim packet sanitizing
- Replaced claim owner names in land-claim permission errors
- Kept real server-side claim ownership untouched
