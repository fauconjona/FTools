resource_manifest_version '05cfa83c-a124-4cfa-a768-c24a5811d8f9'

name 'FTools'
author 'Fauconjona'
description 'Useful tool to create 3D text, Marker, Area and pickups easier'
version '1.0.0'

max_clients '32'

--------------------------------------------------------------------------------

client_script 'FToolsShared.net.dll'
server_script 'FToolsShared.net.dll'

--------------------------------------------------------------------------------

client_script 'FToolsClient.net.dll'
server_script 'FToolsServer.net.dll'
