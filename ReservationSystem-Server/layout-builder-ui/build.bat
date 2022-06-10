@ECHO OFF
ECHO Building react project for layout builder
PUSHD "%~dp0"
CALL npm run build
COPY ".\build\static\js\main.*.js" "..\ReservationSystem-Server\wwwroot\js\layout-builder.gen.js"
POPD
