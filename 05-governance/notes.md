# Notas – Proyecto 05 (Governance & Cost Control)

## Scope en Azure

Una de las cosas más importantes que entendí es que no todo está al mismo nivel.

- Resource Groups → agrupan recursos
- Policies y Budgets → suelen aplicarse a nivel de suscripción

Es fácil confundirse y pensar que todo se controla desde el RG, pero no es así.

---

## Azure Policy (comportamiento real)

### Allowed Locations

Pensaba que iba a bloquear todo, pero no funciona exactamente así.

- Bloquea recursos como Storage, VM, etc.
- No siempre bloquea la creación de Resource Groups

Lo importante es que bloquea el despliegue real, que es lo que tiene impacto.

---

## Deny vs Audit

Esto es clave:

- Deny → bloquea la creación
- Audit → deja crear pero marca incumplimiento

Si algo no se bloquea, lo primero que hay que revisar es esto.

---

## Tags

Al principio parecen un detalle menor, pero no lo son.

Sirven para:
- organizar recursos
- controlar costes
- automatizar procesos

Sin tags, un entorno crece y se vuelve un caos.

---

## Budget

Importante tener claro que:

- No bloquea gasto
- Solo avisa

Es más una herramienta de control que de restricción.

---

## Action Group

Tiene más sentido del que parece:

- Centraliza notificaciones
- Evita repetir configuraciones

En entornos grandes esto ahorra mucho tiempo.

---

## Resource Lock

- CanNotDelete evita borrados accidentales
- Útil en entornos críticos

Pero hay que recordar quitarlo si quieres eliminar el RG, si no te bloqueas a ti mismo.

---

## Errores / cosas que me confundieron

- Pensar que las policies afectaban igual a todos los recursos
- No tener claro el scope de cada cosa
- Creer que el budget podía bloquear gastos

---

## Aprendizaje principal

Aquí ya no se trata solo de crear recursos.

Se trata de:
- controlar lo que se crea
- evitar errores
- limitar riesgos
- entender cómo funciona Azure realmente

---

## Cómo lo explicaría en una entrevista

He trabajado en configurar control de costes mediante budgets y alertas, y en aplicar políticas para restringir despliegues y forzar el uso de tags. También añadí protección con locks y validé todo con pruebas reales para asegurar que las políticas funcionaban como esperaba.