library(dplyr)

space <- read.csv("space.csv", header = TRUE, sep = ",", encoding = "UTF-8")
form <- read.csv("form.csv", header = TRUE, sep = ",", encoding = "UTF-8")

# Juntar las tablas
form_space <- form %>%
  filter(game == "space") 

form_space <- space %>%
  left_join(form_space, by = "userID") %>%
  filter(!is.na(question3))

# Quedarse solo con la última repetición de cada usuario
data <- form_space %>%
  group_by(userID) %>%
  slice_max(order_by = time, n = 1, with_ties = FALSE) %>%
  ungroup()

# Test para H1: aburrimiento (lineal > exponencial)
wilcox.test(composite ~ difficulty, data = data,
            subset = difficulty %in% c("Lineal", "Exponencial"),
            alternative = "greater", var.equal = FALSE)
t.test(composite ~ difficulty, data = data,
       subset = difficulty %in% c("Lineal", "Exponencial"),
       alternative = "greater", var.equal = FALSE)

# Test para H2: frustración (lineal < exponencial)
wilcox.test(question6 ~ difficulty, data = data,
       subset = difficulty %in% c("Lineal", "Exponencial"),
       alternative = "less", var.equal = FALSE)
t.test(question6 ~ difficulty, data = data,
       subset = difficulty %in% c("Lineal", "Exponencial"),
       alternative = "less", var.equal = FALSE)

