import matplotlib.pyplot as plt
import numpy as np
from scipy.interpolate import interp1d

##########BALDOSAS LINEAL#############################################
# Niveles del 1 al 8
niveles = np.arange(1, 6)

# Datos
dificultad = [3, 5, 8, 4, 6, 9, 12, 13]
dificultad = [8,10,13, 14, 16]

# Crear gráfica
plt.figure()
plt.plot(niveles, dificultad, color='tab:blue')
plt.xlabel('Nivel')
plt.ylabel('Dificultad')
plt.title('Aumento de nivel constante (lineal)')
plt.grid(True)
plt.ylim(0,34)
plt.show()

######BALDOSAS EXPONENCIAL########################################
# Niveles del 1 al 5
niveles = np.arange(1, 6)

dificultad = [3, 8, 6, 9, 13]
dificultad = [8,13,  16, 24, 33]

# Crear gráfica
plt.figure()
plt.plot(niveles, dificultad, color='tab:red')
plt.xlabel('Nivel')
plt.ylabel('Dificultad')
plt.title('Aumento de nivel constante (exponencial)')
plt.grid(True)
plt.ylim(0, 34)
plt.show()
