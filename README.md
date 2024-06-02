# ProjectGS-Prototype
Para jugar este juego se puede usar WASD o las flechas direccionales para moverse, el mouse para apuntar, el Shift Izquierdo para dashear en la dirección en la que el personaje se mueve y la Barra Espaciadora para activar el Bullet Time. El personaje dispara automáticamente de acuerdo a su tasa de tiro y va rotando hacia la posición del cursor de acuerdo a una velocidad de rotación. Al quedarse sin munición el personaje iniciará la recarga del arma automáticamente, esto se verá mediante el cambio del icono del cursor.

Eres brevemente invulnerable al daño durante el dash y después de ser golpeado.

Durante el Bullet Time la velocidad de todos los enemigos y la de tu personaje serán reducidas, pero tu velocidad de disparo y recarga no. Aprovecha este recurso para lidiar con enemigos más rápidos con más salud.

La meta del juego es eliminar a los enemigos de las 10 oleadas, fortaleciendose con cada fruta que dejen atrás, que son los powerups de este juego. La manzana aumentará la vida máxima y te curará completamente, el chile aumentará tu munición y tu tasa de disparo,  la banana aumentará tu velocidad de movimiento y tu velocidad de recarga y las uvas aumentarán tu tasa de disparo y tu daño por bala.

Hay tres versiones de un enemigo que te persigue, cada uno de ellos hace más daño y es más rápido que el anterior.

En cuanto a programación, usé objetos programables para los efectos de los powerups y animaciones tanto haciendo uso de Animators como de animación procedimental para la salud y los cooldowns de las habilidades. Para las entidades hice uso de interfaces para categorizar propiedades y demás, instanciando a los enemigos como prefabs en los Assets. Use el nuevo sistema de inputs de Unity para manejar el input del jugador. También hice uso de eventos para disparar eventos conectados de otros scripts. Es posible que hayan algunos vestigios de features que haya tenido la intención de tener, pero que finalmente decidí por cortar para reducir el scope y hacer un poco más eficaz la entrega. 

De antemano, muchas gracias por su tiempo y espero disfruten el prototipo; porque yo disfruté el ejercicio de hacerlo.
