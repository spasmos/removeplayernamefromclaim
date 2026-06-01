# RemovePlayerNameFromClaim

## Titulo

RemovePlayerNameFromClaim

## Descripcion Corta

Mod utilitario server-side que oculta a los clientes el nombre del propietario de los claims de Vintage Story.

## Descripcion Larga

`RemovePlayerNameFromClaim` es un mod ligero del lado del servidor para Vintage Story 1.22.x.

Sustituye por `Unknown` los nombres de propietarios de land claims enviados a los clientes, ayudando a servidores PvP y survival a no revelar informacion gratuita sobre quien controla una zona protegida.

El mod esta pensado para ser muy concreto:

- Solo se ejecuta en el servidor
- Los clientes no necesitan instalarlo
- No modifica la propiedad real guardada en los claims
- Solo cambia lo que ve el cliente en paquetes de claims y mensajes de falta de permisos relacionados con claims

La propiedad real, los permisos y los comandos administrativos siguen funcionando igual en el servidor.

## Instrucciones de Uso

1. Instala el zip del mod en el servidor.
2. Reinicia el servidor.
3. Los claims existentes y nuevos seguiran funcionando normalmente, pero el nombre mostrado al cliente sera `Unknown`.

## Changelog 1.0.0

- Version inicial
- Anonimizados los paquetes de claims enviados desde el servidor
- Sustituidos los nombres de propietario en errores de permisos relacionados con land claims
- Conservada intacta la propiedad real de los claims en el servidor
