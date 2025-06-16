library(dplyr)

tiles <- read.csv("tiles.csv", header = TRUE, sep = ",", encoding = "UTF-8")
form <- read.csv("form.csv", header = TRUE, sep = ",", encoding = "UTF-8")

# Juntar las tablas
form_tiles <- form %>%
  filter(game == "tiles") 

form_tiles <- tiles %>%
  left_join(form_tiles, by = "userID") %>%
  filter(!is.na(question3))

# Quedarse solo con la última repetición de cada usuario
data <- form_tiles %>%
  group_by(userID) %>%
  slice_max(order_by = time, n = 1, with_ties = FALSE) %>%
  ungroup()

# Test para H1: aburrimiento (lineal > exponencial)
t.test(question3 ~ difficulty, data = data,
       subset = difficulty %in% c("Lineal", "Exponencial"),
       alternative = "greater", var.equal = FALSE)

# Test para H2: frustración (lineal < exponencial)
t.test(question6 ~ difficulty, data = data,
       subset = difficulty %in% c("Lineal", "Exponencial"),
       alternative = "less", var.equal = FALSE)
