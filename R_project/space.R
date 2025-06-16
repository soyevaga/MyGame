# Cargar paquetes necesarios
library(ggplot2)
library(dplyr)

# Leer datos
data <- read.csv("space_check.csv", header = TRUE, sep = ",", encoding = "UTF-8")
data$userID <- as.factor(data$userID)

###############ANÁLISIS EXPLORATORIO############################################
summary(data)

# Histogramas
ggplot(data, aes(x = boredom)) +
  geom_histogram(binwidth = 1, fill ="skyblue", color = "black") +
  labs(title = "Histograma de Aburrimiento", x = "Aburrimiento", y = "Frecuencia")

ggplot(data, aes(x = frustration)) +
  geom_histogram(binwidth = 1, fill ="pink", color = "black") +
  labs(title = "Histograma de Frustración", x = "Frustración", y = "Frecuencia")

ggplot(data, aes(x = max_speed)) +
  geom_histogram(binwidth = 1, fill ="lightgreen", color = "black") +
  labs(title = "Histograma de Dificultad", x = "Dificultad", y = "Frecuencia")

# Boxplots
boxplot(data$boredom, main = "Boxplot de Aburrimiento", ylab = "Aburrimiento")
boxplot(data$frustration, main = "Boxplot de Frustración", ylab = "Frustración")
boxplot(data$max_speed, main = "Boxplot de Dificultad", ylab = "Dificultad")

# Gráfico de dispersión
ggplot(data, aes(x = max_speed, y = boredom)) +
  geom_point(color = "blue", alpha = 0.6) +
  geom_smooth(method = "lm", se = FALSE, color = "black") +
  labs(title = "Dispersión: Dificultad vs Aburrimiento",
       x = "Dificultad",
       y = "Aburrimiento")

ggplot(data, aes(x = max_speed, y = frustration)) +
  geom_point(color = "red", alpha = 0.6) +
  geom_smooth(method = "lm", se = FALSE, color = "black") +
  labs(title = "Dispersión: Dificultad vs Frustración",
       x = "Dificultad",
       y = "Frustración")

###############MODELOS Y CORRELACIÓN############################################
# SPEARMAN
cor.test(data$max_speed, data$boredom, method = "spearman", exact = FALSE)
cor.test(data$max_speed, data$frustration, method = "spearman", exact = FALSE)

# Modelo Lineal
modelo_boredom <- lm(max_speed ~ boredom, data = data)
summary(modelo_boredom)
plot(modelo_boredom)

modelo_frustration <- lm(max_speed ~ frustration, data = data)
summary(modelo_frustration)
plot(modelo_frustration)

