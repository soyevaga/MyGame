# Cargar paquetes necesarios
library(ggplot2)
library(dplyr)
library(xtable)

# Leer datos
data <- read.csv("cards_check.csv", header = TRUE, sep = ",", encoding = "UTF-8")
data$userID <- as.factor(data$userID)

###############ANÁLISIS EXPLORATORIO############################################
# Resumen por grupo
resumen <- data %>%
  group_by(difficulty) %>%
  summarise(across(c(boredom, frustration, max_level), 
                   list(media = mean, mediana = median, sd = sd), 
                   na.rm = TRUE))
xtable(resumen, digits = 2)

# Histogramas por grupo
ggplot(data, aes(x = boredom, fill = difficulty)) +
  geom_histogram(binwidth = 1, position = "dodge", color = "black") +
  labs( x = "aburrimiento") +
  theme_minimal()

ggplot(data, aes(x = frustration, fill = difficulty)) +
  geom_histogram(binwidth = 1, position = "dodge", color = "black") +
  labs( x = "frustración") +
  theme_minimal()

ggplot(data, aes(x = max_level, fill = difficulty)) +
  geom_histogram(binwidth = 1, position = "dodge", color = "black") +
  labs(x = "nivel") +
  theme_minimal()
# Export png a 300
par(cex.main = 1.8, cex.axis = 1.5, cex.lab = 1.6)

# Boxplots por grupo
colores <- c("turquoise3", "salmon") 
boxplot(boredom ~ difficulty, data = data, col = colores)
boxplot(frustration ~ difficulty, data = data, col = colores)
boxplot(max_level ~ difficulty, data = data, col = colores)
# Export png a 650

# Gráfico de dispersión
ggplot(data, aes(x = max_level, y = boredom, color = difficulty)) +
  geom_point(alpha = 0.6)  +
  labs(title = "Dispersión: Dificultad vs Aburrimiento", x = "Nivel Máximo", y = "Aburrimiento")

ggplot(data, aes(x = max_level, y = frustration, color = difficulty)) +
  geom_point(alpha = 0.6)  +
  labs(title = "Dispersión: Dificultad vs Frustración", x = "Nivel Máximo", y = "Frustración")


###############MODELOS Y CORRELACIÓN############################################
# SPEARMAN
by(data, data$difficulty, function(sub) {
  list(
    boredom = cor.test(sub$max_level, sub$boredom, method = "spearman", exact = FALSE),
    frustration = cor.test(sub$max_level, sub$frustration, method = "spearman", exact = FALSE)
  )
})

# Modelo Lineal
modelo_boredom <- lm(max_level ~ boredom * difficulty, data = data)
summary(modelo_boredom)
plot(modelo_boredom)

modelo_frustration <- lm(max_level ~ frustration * difficulty, data = data)
summary(modelo_frustration)
plot(modelo_frustration)

# Caracterizar datos
# t-test: solo si distribuye normal
t.test()

# test Kolmogorov - Smirlov: para ver si distribuyen normal o no

# Test Wilcoxon: parecido a tstudent pero en dist no normal