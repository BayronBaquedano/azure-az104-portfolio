# Notes - Laboratorio AZ-104: Hub-Spoke, UDR, Load Balancer y Application Gateway

## 1. Objetivo del laboratorio

El objetivo de este laboratorio fue desplegar y probar una arquitectura de red en Azure basada en el modelo **Hub-Spoke**, validando distintos mecanismos de administración y distribución del tráfico de red.

La prueba incluyó:

- Aprovisionamiento de máquinas virtuales mediante plantilla ARM.
- Configuración de emparejamientos entre redes virtuales.
- Validación de conectividad con Network Watcher.
- Prueba de transitividad entre VNets.
- Configuración de rutas definidas por el usuario, UDR.
- Uso de una máquina virtual como dispositivo virtual de red, NVA.
- Implementación de Azure Load Balancer de capa 4.
- Implementación de Azure Application Gateway de capa 7.

---

## 2. Servicios principales utilizados

- Azure Virtual Network
- Azure Virtual Machines
- Azure Network Interface
- Azure Network Watcher
- Azure Route Table
- User Defined Routes, UDR
- IP Forwarding
- Azure Load Balancer
- Azure Application Gateway
- Public IP Address
- Network Security Groups
- ARM Templates
- PowerShell en Azure Cloud Shell

---

## 3. Arquitectura general

El entorno desplegado está compuesto por varias redes virtuales y máquinas virtuales.

La arquitectura lógica es:

```text
Hub VNet
├── VM0
├── VM1

Spoke VNet 2
└── VM2

Spoke VNet 3
└── VM3
```

La red **Hub** actúa como red central. Las redes **Spoke** se conectan al Hub mediante VNet Peering.

El tráfico entre Spokes no es transitivo automáticamente, por lo que se configuraron rutas definidas por el usuario para forzar el paso del tráfico a través de la VM0, que actúa como router/NVA.

---

## 4. Tarea 1 - Aprovisionamiento del entorno

En la primera tarea se aprovisionó el entorno base usando una plantilla ARM y un archivo de parámetros.

Se definieron variables para:

```powershell
$location1 = "eastus"
$rgName = "az104-06-rg1"
```

Después se creó el grupo de recursos:

```powershell
New-AzResourceGroup -Name $rgName -Location $location1
```

Posteriormente se desplegó la plantilla ARM:

```powershell
New-AzResourceGroupDeployment `
  -ResourceGroupName $rgName `
  -TemplateFile az104-06-template.json `
  -TemplateParameterFile az104-06-parameters.json
```

El despliegue creó las redes virtuales, máquinas virtuales, tarjetas de red y recursos necesarios para el laboratorio.

---

## 5. Instalación de la extensión de Network Watcher

Después del aprovisionamiento, se instaló la extensión de Network Watcher en las máquinas virtuales.

El objetivo de esta extensión fue permitir pruebas de conectividad desde Azure, especialmente mediante **Connection Troubleshoot**.

Esto permite validar si una máquina virtual puede alcanzar otra máquina virtual por un puerto específico, por ejemplo RDP en el puerto 3389.

---

## 6. Tarea 2 - Configuración de topología Hub-Spoke

Se configuraron emparejamientos de red virtual para crear la topología Hub-Spoke.

Los peerings principales fueron:

```text
VNet1 <-> VNet2
VNet1 <-> VNet3
```

En los peerings se permitió:

- Tráfico hacia la red virtual remota.
- Tráfico reenviado, forwarded traffic.

No se configuró gateway en esta fase.

Punto clave:

```text
VNet Peering no es transitivo por defecto.
```

Esto significa que aunque VNet2 esté conectada al Hub y VNet3 también esté conectada al Hub, VNet2 no puede comunicarse automáticamente con VNet3 si no se configura enrutamiento adicional.

---

## 7. Tarea 3 - Prueba de conectividad con Network Watcher

Se utilizó Network Watcher para probar la conectividad entre máquinas virtuales.

Pruebas realizadas:

```text
VM0 -> VM1 : Reachable
VM0 -> VM2 : Reachable
VM0 -> VM3 : Reachable
VM2 -> VM3 : Not reachable
```

El resultado esperado fue que VM2 no pudiera alcanzar VM3 directamente, ya que no existía peering directo entre sus VNets y el peering Hub-Spoke no proporciona transitividad automática.

Esta prueba confirma un concepto importante para AZ-104:

```text
El VNet Peering conecta redes, pero no convierte automáticamente el Hub en router transitivo.
```

---

## 8. Tarea 4 - Configuración de enrutamiento con UDR

Para permitir comunicación entre los Spokes, se configuró enrutamiento manual usando **User Defined Routes**.

Primero se habilitó **IP Forwarding** en la NIC de VM0.

VM0 pasó a actuar como una NVA, Network Virtual Appliance.

También se instalaron roles de Windows necesarios para habilitar el enrutamiento dentro del sistema operativo.

### Ruta de VNet2 hacia VNet3

Se creó una tabla de rutas con una ruta hacia el espacio de direcciones de VNet3.

Ejemplo conceptual:

```text
Destino: 10.63.0.0/20
Next hop type: Virtual appliance
Next hop IP: 10.60.0.4
```

Después se asoció la tabla de rutas a la subnet correspondiente de VNet2.

### Ruta de VNet3 hacia VNet2

Se creó otra tabla de rutas con una ruta hacia el espacio de direcciones de VNet2.

Ejemplo conceptual:

```text
Destino: 10.62.0.0/20
Next hop type: Virtual appliance
Next hop IP: 10.60.0.4
```

Después se asoció la tabla de rutas a la subnet correspondiente de VNet3.

### Resultado esperado

Después de configurar las UDR, VM2 pudo comunicarse con VM3 pasando por VM0.

Flujo lógico:

```text
VM2
↓
Route Table
↓
VM0 como NVA
↓
VM3
```

Concepto clave:

```text
Las UDR permiten controlar explícitamente el camino del tráfico dentro de Azure.
```

---

## 9. Tarea 5 - Implementación de Azure Load Balancer

Se implementó un **Azure Load Balancer público** delante de dos máquinas virtuales.

Este balanceador trabaja en **capa 4**, es decir, distribuye tráfico basado en protocolo y puerto, como TCP o UDP.

Configuración principal:

- Tipo: Public Load Balancer
- SKU: Standard
- Frontend IP pública
- Backend Pool con VM0 y VM1
- Health Probe TCP puerto 80
- Load Balancing Rule TCP puerto 80

Flujo:

```text
Internet
↓
Public IP del Load Balancer
↓
Frontend configuration
↓
Load balancing rule
↓
Backend pool
↓
VM0 / VM1
```

Se validó el acceso desde navegador usando la IP pública del Load Balancer.

Concepto clave:

```text
Azure Load Balancer distribuye tráfico a nivel de transporte, no entiende rutas HTTP, dominios ni contenido web.
```

---

## 10. Tarea 6 - Implementación de Azure Application Gateway

Se implementó un **Azure Application Gateway** delante de máquinas virtuales ubicadas en la red Spoke.

Application Gateway trabaja en **capa 7**, es decir, puede tomar decisiones basadas en HTTP/HTTPS.

Antes de crearlo, se añadió una subnet dedicada para Application Gateway.

Importante:

```text
Application Gateway requiere una subnet dedicada.
```

Configuración principal:

- SKU: Standard_v2
- Frontend público
- Public IP nueva
- Backend Pool con IPs privadas de servidores
- Listener HTTP puerto 80
- Backend settings HTTP puerto 80
- Routing Rule con prioridad

Flujo:

```text
Internet
↓
Public IP de Application Gateway
↓
Listener HTTP
↓
Routing Rule
↓
Backend Pool
↓
VMs backend
```

Se validó el acceso desde navegador usando la IP pública del Application Gateway.

Concepto clave:

```text
Application Gateway es un balanceador de capa 7 orientado a aplicaciones web.
```

---

## 11. Diferencia entre Azure Load Balancer y Application Gateway

| Servicio | Capa | Uso principal |
|---|---:|---|
| Azure Load Balancer | 4 | Balancear tráfico TCP/UDP |
| Azure Application Gateway | 7 | Balancear tráfico HTTP/HTTPS |

Azure Load Balancer se usa cuando solo interesa distribuir tráfico por puerto y protocolo.

Application Gateway se usa cuando se necesita lógica de aplicación, como listeners HTTP, reglas por ruta, hostnames, SSL termination o WAF.

---

## 12. Conceptos clave aprendidos

- Una topología Hub-Spoke permite centralizar conectividad y control de red.
- VNet Peering no es transitivo por defecto.
- Para enrutar tráfico entre Spokes se necesitan UDR o soluciones como Azure Firewall/NVA.
- IP Forwarding debe habilitarse en la NIC de una VM que actúe como router.
- El sistema operativo de la VM también debe estar configurado para reenviar tráfico.
- Route Tables se asocian a subnets, no directamente a VMs.
- Network Watcher permite validar conectividad y diagnosticar problemas de red.
- Azure Load Balancer trabaja en capa 4.
- Azure Application Gateway trabaja en capa 7.
- Application Gateway necesita una subnet dedicada.
- El SKU Standard de Load Balancer es el recomendado para escenarios reales.

---

## 13. Errores y puntos de atención

### 1. Confundir peering con transitividad

Tener peering entre Hub y Spokes no significa que los Spokes puedan hablar entre sí automáticamente.

### 2. Crear UDR sin asociarlas a una subnet

Una route table sin asociación no afecta al tráfico.

### 3. Habilitar IP Forwarding solo en Azure

Además de habilitar IP Forwarding en la NIC, también hay que configurar el sistema operativo para enrutar tráfico.

### 4. Confundir Load Balancer con Application Gateway

Load Balancer no interpreta tráfico HTTP. Application Gateway sí.

### 5. No crear subnet dedicada para Application Gateway

Application Gateway requiere una subnet propia.

---

## 14. Conclusión

Este laboratorio permitió practicar una arquitectura de red más cercana a escenarios reales de empresa.

Los puntos más importantes fueron la creación de una topología Hub-Spoke, la validación de la no transitividad del peering, la configuración de rutas definidas por el usuario y la comparación práctica entre Azure Load Balancer y Azure Application Gateway.

Este tipo de práctica es especialmente relevante para AZ-104 porque conecta varios temas críticos del examen:

- redes virtuales
- emparejamiento de redes
- rutas
- balanceo de carga
- diagnóstico de conectividad
- administración de máquinas virtuales
