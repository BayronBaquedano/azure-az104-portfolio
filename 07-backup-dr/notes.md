# Notas – Proyecto 07 (Azure Backup)

## Concepto principal

Azure Backup usa un recurso llamado **Recovery Services Vault** para almacenar los puntos de recuperación.

La VM no guarda el backup en sí misma.

---

## Flujo de Azure Backup

VM → Backup Policy → Recovery Services Vault → Recovery Points → Restore

---

## Policy de backup

Define dos cosas importantes:

- Frecuencia (cada cuánto se hace el backup)
- Retención (cuánto tiempo se guardan)

En este proyecto se usó:

- Backup diario
- Retención de 7 días

---

## Backup manual

Aunque exista una policy, es útil ejecutar **Backup Now** para crear un recovery point inmediatamente.

Esto permite probar restauraciones sin esperar al horario programado.

---

## Restore

Azure permite diferentes tipos de restauración:

- Restore VM completa
- Restore discos
- Restore archivos (según configuración)

En este proyecto se probó **Restore Disks**, que es más rápido y suficiente para validar recuperación.

---

## Aprendizaje clave

Backup no es solo guardar datos.

Lo importante es **poder recuperar el sistema cuando algo falla**.