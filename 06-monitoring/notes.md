# Notas – Proyecto 06 (Monitoring)

## Concepto clave

Azure Monitor no funciona todo “automático”.

Hay distintos niveles:

1. Métricas básicas (CPU, red)
2. Logs básicos (Heartbeat)
3. Logs avanzados (Perf, Syslog → requieren configuración adicional)

---

## Heartbeat

- Es la forma más simple de comprobar que la VM está conectada.
- Si no aparece → algo está mal en la conexión o agente.

Query usada:

Heartbeat | take 10

---

## Alertas

- Son más importantes que los logs en la práctica.
- Permiten reaccionar automáticamente.

Ejemplo:
- CPU > 20% durante 1 minuto (para pruebas)

---

## Problema encontrado

Intenté usar la tabla `Perf`:

Error:
Failed to resolve table 'Perf'

Motivo:
- No había configurado Performance Counters en una DCR.

Aprendizaje:
- En Azure, si no configuras la fuente de datos → la tabla no existe.

---

## Lección importante

No todo lo que existe en KQL está disponible por defecto.

Hay que entender:
- Qué datos se están recogiendo
- De dónde vienen

---

## Error personal

- Intentar avanzar sin entender el flujo completo
- Mezclar configuraciones (DCR, insights, diagnostic settings)

---

## Aprendizaje real

Monitoring no es:
“ver datos”

Es:
- entender qué datos tienes
- configurar qué datos necesitas
- reaccionar ante ellos

---

## Cómo lo explicaría

Configuré monitorización básica en Azure usando Azure Monitor, verifiqué la conexión mediante Heartbeat y creé alertas basadas en CPU para detectar uso elevado y recibir notificaciones.