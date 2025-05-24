import matplotlib.pyplot as plt
import numpy as np

##########METEORITOS LINEAL############################
# Niveles de 1 a 9
niveles = np.arange(1, 10)

# Velocidad: hacemos que a nivel 1 valga 2
velocidad = 2 + (niveles-1)

# Crear gráfica
plt.figure()
plt.plot(niveles, velocidad, color='tab:blue')
plt.xlabel('Nivel')
plt.ylabel('Velocidad')
plt.title('Aumento de nivel constante (lineal)')
plt.grid(True)
plt.ylim(0, 60)
plt.show()

##########METEORITOS EXPONENCIAL############################

# Niveles de 1 a 9
niveles = np.arange(1, 10)

# velocidad = 1.5^(2 + i)
velocidad = 1.5 ** (niveles+1)

# Crear gráfica
plt.figure()
plt.plot(niveles, velocidad, color='tab:red')
plt.xlabel('Nivel')
plt.ylabel('Velocidad')
plt.title('Aumento de nivel constante (exponencial)')
plt.grid(True)
plt.ylim(0, 60)
plt.show()