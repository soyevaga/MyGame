library(dplyr) 

# Leer tablas
form <- read.csv("form.csv", header = TRUE, sep = ",", encoding = "UTF-8")
users <- read.csv("users.csv", header = TRUE, sep = ",", encoding = "UTF-8")


cards <- read.csv("cards.csv", header = TRUE, sep = ",", encoding = "UTF-8")
space <- read.csv("space.csv", header = TRUE, sep = ",", encoding = "UTF-8")
tiles <- read.csv("tiles.csv", header = TRUE, sep = ",", encoding = "UTF-8")

# userID es string
form$userID <- as.factor(form$userID)
users$userID <- as.factor(users$userID)

space$userID <- as.factor(space$userID)
cards$userID <- as.factor(cards$userID)
tiles$userID <- as.factor(tiles$userID)

# Unir ambas
form_users <- form %>%
  left_join(users, by = "userID")
form_users$userID <- as.character(form_users$userID)

# Dividir según paridad
form_par <- filter(form_users, birthday == "Par")
form_impar <- filter(form_users, birthday == "Impar")

## prueba:
data <- form_users[13:28,]
View(data)

summary(data)

##DESCRIPTIVO NUMÉRICO
boxplot(question3 ~ gender, data = data)

hombres <- data[,data$gender == "Hombre"]
mujeres <- data[,data$gender == "Mujer"]

t.test(x=hombres$question3, y=mujeres$question3, mu = 0)
t.test(x=hombres$question3, y=mujeres$question3, mu = 0, alternative = "less")

modelo <- lm(question3 ~ gender, data = data)
summary(modelo)
