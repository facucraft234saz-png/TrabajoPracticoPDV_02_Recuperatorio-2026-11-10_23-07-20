# Trabajo Práctico 02 – Recuperatorio / Programación de Videojuegos I

## Descripción

Este proyecto corresponde al **Trabajo Práctico N.º 02 – Recuperatorio de Programación de Videojuegos I**.

El objetivo fue desarrollar un **prototipo de videojuego 3D de infiltración y sigilo utilizando Unity 6**, aplicando conceptos de programación, inteligencia artificial, navegación mediante NavMesh, detección por Raycast y LayerMask, sistemas de interacción, administración de estados y mecánicas de supervivencia.

El juego se desarrolla en las instalaciones de un laboratorio de seguridad. El jugador debe infiltrarse en el edificio principal, recorrer el escenario y atravesar diferentes zonas peligrosas hasta llegar a una habitación donde se encuentra la computadora que debe hackear.

Durante el recorrido deberá evitar o esquivar los drones de seguridad, las trampas de rayos láser y las zonas de gas tóxico. También podrá recolectar datos para aumentar su puntaje, utilizar un potenciador de invisibilidad para evitar ser detectado y recuperar vida en una habitación equipada con una estación sanadora.

Una vez completado el hackeo de la computadora, el jugador deberá abandonar las instalaciones y regresar hasta la plataforma de extracción. Al pisarla, el sistema verificará que el objetivo principal haya sido completado y activará la victoria.

El proyecto incorpora las mecánicas principales solicitadas para el recuperatorio, incluyendo un sistema de vida y daño, puntuación, inteligencia artificial enemiga, objetos interactivos, poderes especiales, trampas ambientales y administración centralizada de la partida.

---

## Versión de Unity

**Unity 6 – 6000.3.22f1**

---

## Controles

| Tecla | Acción |
| :--- | :--- |
| **W / A / S / D** | Movimiento del jugador |
| **Espacio** | Saltar |
| **Flecha Izquierda / Derecha** | Girar la cámara horizontalmente |
| **E** | Interactuar con puertas y computadora |
| **ESC** | Pausar o reanudar la partida |

---

## Mecánicas implementadas

### 1. Movimiento y control del jugador

- Movimiento mediante las teclas W, A, S y D.
- Salto mediante la tecla Espacio.
- Control horizontal de la cámara mediante las flechas izquierda y derecha.
- Movimiento mediante Rigidbody para gestionar el desplazamiento y las colisiones con el escenario.
- Detección del suelo mediante Raycast para comprobar si el jugador puede saltar.

<img width="757" height="327" alt="image" src="https://github.com/user-attachments/assets/b284d87a-01da-4bae-8705-ee269518f7b2" />


### 2. Diseño del escenario mediante Terrain Tool

- Creación de un terreno 3D utilizando las herramientas de Terrain de Unity.
- Aplicación de diferentes texturas para representar las superficies.
- Construcción de desniveles y variaciones de altura.
- Distribución de obstáculos y estructuras para representar el exterior del laboratorio.

También se construyó un edificio principal que contiene las habitaciones y zonas por las que debe avanzar el jugador, junto con un laberinto que dificulta el recorrido y permite distribuir los diferentes peligros.

<img width="661" height="339" alt="image" src="https://github.com/user-attachments/assets/a5da8309-9921-4127-8d9c-4e47652d00ab" />


### 3. Raycast y LayerMask para la interacción con objetos

- Implementación de Raycast desde la cámara del jugador.
- Utilización de LayerMask para detectar únicamente objetos pertenecientes a la capa interactiva.
- Exclusión de objetos que no corresponden a la interacción.
- Uso de la tecla E para activar los objetos detectados.
- Implementación del sistema de interacción en puertas y en la computadora que debe hackearse.

La detección mediante Raycast permite que el jugador interactúe con objetos específicos sin tener que comprobar todos los elementos del escenario.

### 4. Puerta interactiva con Raycast y LayerMask

- La puerta se detecta mediante un Raycast desde la cámara del jugador.
- Se utiliza una LayerMask para filtrar los objetos interactivos.
- Al presionar E, la puerta comienza a reducir progresivamente su escala hasta desaparecer.
- Permanece desaparecida durante 20 segundos.
- Después del tiempo establecido, recupera su escala original y vuelve a estar disponible.
- Se impiden interacciones repetidas mientras se encuentra en el proceso de desaparición o espera.

Esta mecánica permite aplicar el sistema de interacción solicitado mediante Raycast y LayerMask a un objeto diferente de la computadora.

<img width="510" height="222" alt="image" src="https://github.com/user-attachments/assets/efca6ae9-1498-4eed-83ab-9b4612f4c881" />

<img width="689" height="324" alt="image" src="https://github.com/user-attachments/assets/2e4b3ce6-c4c2-473a-9c6b-a325476e984b" />

### 5. Computadora y objetivo principal de hackeo

- El jugador debe llegar hasta la habitación donde se encuentra la computadora.
- La interacción se realiza mediante la tecla E.
- Al completar el proceso de hackeo, se actualiza el estado del objetivo.
- La plataforma de extracción comprueba si la computadora fue hackeada antes de permitir la victoria.

El hackeo es el objetivo principal de la misión y condiciona el resultado final de la partida.

<img width="502" height="257" alt="image" src="https://github.com/user-attachments/assets/2a290550-1752-4ac2-a355-3e75db32cdae" />


### 6. Inteligencia artificial de los drones mediante NavMesh

- Implementación de drones de seguridad utilizando `NavMeshAgent`.
- Patrullaje automático entre puntos establecidos.
- Detección del jugador dentro del rango correspondiente.
- Persecución automática cuando el jugador es detectado.
- Ataque al jugador cuando se encuentra dentro del rango de ataque.
- Retorno al comportamiento de patrullaje cuando deja de detectar al objetivo.

Los drones representan una amenaza durante la infiltración y obligan al jugador a desplazarse con cuidado por las instalaciones.

<img width="417" height="155" alt="image" src="https://github.com/user-attachments/assets/06dad8c5-09bc-4414-bae9-1957ebb9cc90" />

<img width="369" height="296" alt="image" src="https://github.com/user-attachments/assets/c54c4cad-cf89-4fbe-874f-6c92efeb0d31" />


### 7. Sistema de vida y daño

- El jugador cuenta con un sistema de puntos de vida.
- Los drones pueden causar daño mediante sus ataques o contacto.
- Se implementa un tiempo de inmunidad entre determinados impactos para evitar recibir daño continuamente.
- Las trampas y zonas ambientales también pueden afectar la vida del jugador.
- La interfaz permite visualizar la vida actual y el puntaje durante la partida.
- Al llegar a cero puntos de vida, se activa el estado de derrota.

<img width="177" height="81" alt="image" src="https://github.com/user-attachments/assets/e1fddd36-c648-4dfc-a5d2-ed52e6c8463a" />

<img width="501" height="237" alt="image" src="https://github.com/user-attachments/assets/9dac74ed-d97c-485d-b554-0660c907da21" />

### 8. Sistema de puntuación y recolección de datos

- Distribución de objetos coleccionables por el escenario.
- Detección del jugador mediante colisiones y triggers.
- Incremento del puntaje al recoger cada objeto.
- Desactivación del objeto una vez recolectado.
- Registro del puntaje actual mediante el sistema de puntuación.
- Visualización del puntaje en la interfaz del jugador.

Los objetos coleccionables representan datos que el jugador puede recuperar durante la infiltración.
<img width="118" height="143" alt="image" src="https://github.com/user-attachments/assets/72da391f-2cc2-40fa-90c4-0309aac3563d" />

### 9. Potenciador de invisibilidad

- Implementación de un objeto coleccionable que activa la invisibilidad del jugador.
- Aplicación de un material configurable desde el Inspector.
- Duración temporal de la invisibilidad.
- Modificación de la velocidad del jugador durante el efecto, con un aumento del 20 %.
- Visualización del tiempo restante mediante un temporizador en la interfaz.
- Al finalizar el efecto, se restauran los materiales originales y la velocidad normal.
- El potenciador reaparece después de 20 segundos para poder volver a utilizarse.

Además, el sistema de detección de los drones tiene en cuenta el estado de invisibilidad del jugador, evitando que lo persigan mientras el efecto se encuentra activo.

<img width="135" height="189" alt="image" src="https://github.com/user-attachments/assets/eb60ca6e-8a28-4522-9cb2-42da64c26844" />
<img width="466" height="190" alt="image" src="https://github.com/user-attachments/assets/4e681562-9ae1-435a-951c-aec73c37ed41" />


### 10. Trampa de rayos láser

- Construcción de una trampa ambiental mediante un objeto visual que representa un rayo láser.
- Utilización de un Collider configurado como Trigger para detectar al jugador.
- Aplicación de daño cuando el jugador permanece dentro de la zona de peligro.
- Implementación de un intervalo entre aplicaciones de daño.

Esta mecánica obliga al jugador a observar el entorno y evitar las zonas protegidas por los sistemas de seguridad.

<img width="374" height="238" alt="image" src="https://github.com/user-attachments/assets/ff3eddcc-2d96-496f-83f5-bd9fdc3a6b98" />


### 11. Zona de gas tóxico

- Creación de una zona ambiental peligrosa mediante un Collider Trigger.
- Incorporación de partículas para representar visualmente el gas.
- Detección del jugador al entrar en la zona.
- Aplicación de daño ambiental de forma periódica mientras permanece dentro.
- Detección de salida de la zona para detener el efecto.

El gas tóxico funciona como un obstáculo ambiental que reduce la vida del jugador si atraviesa o permanece dentro del área afectada.
<img width="210" height="216" alt="image" src="https://github.com/user-attachments/assets/8920645a-cd86-497d-b3d9-6f822d9ffce2" />

### 12. Habitación y estación sanadora

- Construcción de una habitación destinada a recuperar vida.
- Implementación de una estación sanadora con elementos visuales, iluminación y partículas.
- Detección del jugador mediante un Collider configurado como Trigger.
- Recuperación progresiva de 10 puntos de vida cada 1,5 segundos.
- Límite de cargas disponibles para evitar el uso ilimitado.
- Activación de un tiempo de espera cuando se agotan las cargas.
- Desactivación de los efectos visuales durante el período de recarga y posterior restauración.

Esta mecánica cumple con el requisito obligatorio de implementar una estación de curación con uso limitado, recuperación progresiva y tiempo de recarga.

<img width="163" height="223" alt="image" src="https://github.com/user-attachments/assets/fe25c045-9e09-4cfd-968c-2760122a8cd3" />


### 13. Plataforma de extracción y condición de victoria

- Implementación de una plataforma que funciona como punto de extracción.
- Detección del jugador mediante Collider y Trigger.
- Comprobación del estado de hackeo de la computadora.
- Si el objetivo no fue completado, la plataforma no permite ganar.
- Si la computadora fue hackeada, se activa la condición de victoria.
- Comunicación del resultado al GameManager.

El jugador debe completar el hackeo y regresar a la plataforma para finalizar correctamente la misión.

<img width="476" height="188" alt="image" src="https://github.com/user-attachments/assets/21d1ecc7-cc56-4de8-9a08-59a0da7cd513" />
<img width="412" height="253" alt="image" src="https://github.com/user-attachments/assets/5ad49ca0-4011-4579-8d0b-4235dbe17cf5" />
<img width="363" height="225" alt="image" src="https://github.com/user-attachments/assets/22a98130-f206-4504-ac44-03a289c6a0c7" />

### 14. GameManager y estados de la partida

Se implementó un GameManager utilizando el patrón Singleton para centralizar el control de la partida.

Los estados principales son:

- **Start:** inicialización de la partida.
- **Playing:** estado normal durante el juego.
- **Paused:** pausa de la partida mediante ESC.
- **Victory:** se activa cuando el jugador completa la misión.
- **GameOver:** se activa cuando el jugador pierde todos sus puntos de vida.

Funciones implementadas:

- Administración centralizada de los estados.
- Suscripción a los eventos de vida y puntuación.
- Registro de cambios de estado, vida y puntaje mediante `Debug.Log()`.
- Congelación del tiempo utilizando `Time.timeScale` al pausar.
- Visualización de los paneles de victoria y derrota.
- Cuenta regresiva de 10 segundos después de ganar o perder.
- Reinicio automático de la escena al finalizar la cuenta regresiva.

El sistema utiliza tiempo no escalado para que la cuenta regresiva de reinicio continúe funcionando aunque el juego se encuentre congelado.

<img width="766" height="322" alt="image" src="https://github.com/user-attachments/assets/5b8f2f20-436f-4163-bf6f-1239e735ce8c" />

<img width="435" height="286" alt="image" src="https://github.com/user-attachments/assets/a5849cc0-66df-4ecb-a60f-3777df4952ff" />

<img width="545" height="328" alt="image" src="https://github.com/user-attachments/assets/927bf1cc-1a74-47f4-ad41-43da323aac41" />


---


A continuación se relacionan los requisitos solicitados en la consigna con las implementaciones realizadas en el proyecto.

| Requisito solicitado | Implementación realizada |
| :--- | :--- |
| Raycast para detectar el suelo | Raycast desde los pies del jugador para comprobar si puede saltar. |
| Raycast frontal para interactuar | Sistema de interacción desde la cámara del jugador. |
| LayerMask | Filtrado de los objetos pertenecientes a la capa interactiva. |
| Evitar detectar el propio jugador | Configuración del origen, distancia y capas de detección para evitar impactos innecesarios sobre el propio jugador. |
| Construcción de NavMesh | Preparación de las superficies navegables del escenario. |
| Enemigo con NavMeshAgent | Drones de seguridad con navegación automática. |
| Patrullaje y persecución | Los drones recorren puntos establecidos y persiguen al jugador cuando lo detectan. |
| Ataque de enemigos | Los drones pueden causar daño cuando el jugador se encuentra dentro del rango de ataque. |
| Sistema de vida y daño | Control de vida, recepción de daño e inmunidad temporal entre impactos. |
| Sistema de puntuación | Recolección de datos y actualización del puntaje. |
| GameManager Singleton | Administración centralizada de los estados de la partida. |
| Estados del juego | Start, Playing, Paused, Victory y GameOver. |
| Pausa | Uso de ESC y `Time.timeScale` para congelar y reanudar el juego. |
| Estación de curación obligatoria | Habitación sanadora con recuperación progresiva, cargas limitadas y tiempo de recarga. |

### Mecánicas opcionales seleccionadas

Se implementaron las siguientes tres mecánicas adicionales:

- [x] **Invisibilidad:** potenciador temporal que modifica los materiales del jugador, aumenta su velocidad y evita que los drones lo detecten durante el efecto.
- [x] **Trampa láser:** zona de peligro que detecta al jugador y aplica daño mediante un Trigger.
- [x] **Gas tóxico:** zona ambiental con partículas que aplica daño periódico mientras el jugador permanece dentro.

---

## Conceptos de programación utilizados

Durante el desarrollo se aplicaron los siguientes conceptos y herramientas:

- **Variables, métodos y referencias:** organización de los datos y comportamientos de cada sistema.
- **Corrutinas (`IEnumerator`):** implementación de temporizadores, hackeo, desaparición de puertas, duración de poderes y recarga de la estación sanadora.
- **`Time.deltaTime`:** control de movimientos y procesos dependientes del tiempo.
- **`Time.unscaledDeltaTime`:** actualización de la cuenta regresiva de reinicio independientemente de la pausa.
- **`Physics.Raycast()`:** detección frontal de objetos interactivos y comprobación del suelo.
- **`LayerMask`:** filtrado de capas para limitar las detecciones.
- **`NavMeshAgent`:** navegación, patrullaje y persecución de los drones.
- **`OnTriggerEnter()`, `OnTriggerStay()` y `OnTriggerExit()`:** detección de recolección, zonas de peligro, curación y extracción.
- **`Rigidbody`:** control físico del jugador.
- **`Vector3.Lerp()`:** transición progresiva de la escala de la puerta.
- **`Mathf.Clamp()` y `Mathf.Clamp01()`:** limitación y control de valores.
- **`Time.timeScale`:** pausa y congelación del juego.
- **Eventos y propiedades:** comunicación entre los sistemas de vida, puntuación y GameManager.
- **Patrón Singleton:** acceso centralizado al GameManager.
- **`GameObject.SetActive()`:** activación y desactivación de objetos y elementos de interfaz.
- **`SceneManager.LoadScene()`:** reinicio de la escena.
- **TextMeshPro e interfaz de usuario:** visualización de vida, puntaje, temporizadores y paneles de estado.

---

## Objetivo del juego

El objetivo principal es **infiltrarse en el laboratorio, evitar sus sistemas de seguridad, llegar hasta la habitación de la computadora, completar el hackeo y regresar a la plataforma de extracción**.

Para completar la misión, el jugador deberá:

1. Ingresar al edificio principal y recorrer sus instalaciones.
2. Esquivar los drones de seguridad y evitar sus ataques.
3. Atravesar el laberinto y las diferentes zonas peligrosas.
4. Evitar las trampas de rayos láser y el gas tóxico.
5. Utilizar los objetos coleccionables y la estación sanadora para administrar sus recursos.
6. Llegar a la habitación donde se encuentra la computadora.
7. Interactuar con ella mediante la tecla E y completar el hackeo.
8. Abandonar la habitación y regresar hasta la plataforma de extracción.
9. Pisar la plataforma para activar la condición de victoria.

Si el jugador pierde todos sus puntos de vida, se activa el estado GameOver. Si completa el hackeo y alcanza la plataforma de extracción, se activa Victory.

Después de cualquiera de estos resultados, el GameManager muestra el panel correspondiente y reinicia automáticamente la escena luego de 10 segundos.

---

## Tareas y mecánicas completadas

- [x] Movimiento del jugador mediante WASD.
- [x] Salto mediante Espacio.
- [x] Control horizontal de cámara mediante flechas izquierda y derecha.
- [x] Creación del terreno mediante Terrain Tool.
- [x] Construcción del edificio principal y sus habitaciones.
- [x] Construcción del laberinto.
- [x] Raycast para detección del suelo.
- [x] Raycast frontal para interacción.
- [x] Filtrado de objetos mediante LayerMask.
- [x] Interacción con puertas mediante E.
- [x] Puerta que desaparece progresivamente y reaparece después de 20 segundos.
- [x] Computadora interactiva y sistema de hackeo.
- [x] Construcción y configuración del NavMesh.
- [x] Drones con NavMeshAgent.
- [x] Patrullaje y persecución de enemigos.
- [x] Ataques y daño de los drones.
- [x] Sistema de vida del jugador.
- [x] Sistema de inmunidad temporal al daño.
- [x] Recolección de datos y sistema de puntuación.
- [x] Potenciador de invisibilidad.
- [x] Aumento de velocidad durante la invisibilidad.
- [x] Temporizador visual del potenciador.
- [x] Reaparición del potenciador después de 20 segundos.
- [x] Trampas de rayos láser.
- [x] Zona de gas tóxico con partículas y daño periódico.
- [x] Habitación sanadora.
- [x] Estación de curación con cargas limitadas y recarga.
- [x] Plataforma de extracción con verificación del hackeo.
- [x] GameManager mediante Singleton.
- [x] Estados Start, Playing, Paused, Victory y GameOver.
- [x] Menú de pausa mediante ESC.
- [x] Paneles de victoria y derrota.
- [x] Reinicio automático después de 10 segundos.
- [x] Registro de estados, vida y puntaje mediante Debug.Log.

---

## Autor

**Nombre y apellido:** [Robert Fuentes Facundo Guillermo]  
**DNI:** [46597991]  
**Materia:** Programación de Videojuegos I  
**Trabajo:** TP2 – Recuperatorio 2026

---

## Conclusión

El desarrollo de este recuperatorio permitió integrar diferentes herramientas de Unity en un único prototipo de videojuego 3D.

Se implementaron sistemas de movimiento, detección mediante Raycast y LayerMask, navegación de enemigos con NavMesh, administración de vida y puntuación, interacción con objetos, trampas ambientales, potenciadores y gestión de estados mediante un GameManager.

Además, se incorporó la estación sanadora obligatoria y se seleccionaron tres mecánicas opcionales: invisibilidad, trampa láser y gas tóxico.

El resultado es un prototipo de infiltración en el que el jugador debe administrar sus recursos, evitar los sistemas de seguridad y completar una misión con un objetivo y una condición de victoria definidos.
