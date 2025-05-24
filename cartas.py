import matplotlib.pyplot as plt
import numpy as np

##########CARTAS LINEAL#############################################
# Niveles del 1 al 8
niveles = np.arange(1, 9)

# Datos
cartas = [3,4,5,6,7,8,9,10]

import matplotlib.pyplot as plt
import numpy as np

# Niveles del 1 al 8
niveles = np.arange(1, 9)

# Datos
cartas = [0,1 ,2 ,3, 4, 5, 6, 7]

# Crear gráfica
plt.figure()
plt.plot(niveles, cartas, color='tab:blue')
plt.xlabel('Nivel')
plt.ylabel('Dificultad')
plt.title('Aumento de nivel constante (lineal)')
plt.grid(True)
plt.ylim(0, 18)
plt.show()

######CARTAS EXPONENCIAL########################################
# Niveles del 1 al 8
niveles = np.arange(1, 9)

# Datos
cartas = [0, 1, 2, 3, 5,8,11,17]


# Crear gráfica
plt.figure()
plt.plot(niveles, cartas, color='tab:red')
plt.xlabel('Nivel')
plt.ylabel('Dificultad')
plt.title('Aumento de nivel constante (exponencial)')
plt.grid(True)
plt.ylim(0, 18)
plt.show()
