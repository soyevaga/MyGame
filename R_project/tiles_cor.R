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
form_tiles <- form_tiles %>%
  group_by(userID) %>%
  slice_max(order_by = time, n = 1, with_ties = FALSE) %>%
  ungroup()

form_tiles_lineal <- subset(form_tiles, difficulty == "Lineal")
form_tiles_exponencial <- subset(form_tiles, difficulty == "Exponencial")

###################################################################
data <- form_tiles_lineal
data <- form_tiles_exponencial

cor.test(data$question3, data$clicks, method = "pearson")
cor.test(data$question6, data$clicks, method = "pearson")
cor.test(data$question3, data$level, method = "pearson")
cor.test(data$question6, data$level, method = "pearson")
cor.test(data$question3, data$restarts, method = "pearson")
cor.test(data$question6, data$restarts, method = "pearson")
cor.test(data$question3, data$arrows_deleted, method = "pearson")
cor.test(data$question6, data$arrows_deleted, method = "pearson")

cor.test(data$question3, data$clicks, method = "spearman")
cor.test(data$question6, data$clicks, method = "spearman")
cor.test(data$question3, data$level, method = "spearman")
cor.test(data$question6, data$level, method = "spearman")
cor.test(data$question3, data$restarts, method = "spearman")
cor.test(data$question6, data$restarts, method = "spearman")
cor.test(data$question3, data$arrows_deleted, method = "spearman")
cor.test(data$question6, data$arrows_deleted, method = "spearman")

###########LINEAL###################################################
data <- form_tiles_lineal
# Correlación con clicks
cor.test(data$question1, data$clicks, method = "pearson")
cor.test(data$question2, data$clicks, method = "pearson")
cor.test(data$question3, data$clicks, method = "pearson")
cor.test(data$question4, data$clicks, method = "pearson")
cor.test(data$question5, data$clicks, method = "pearson")
cor.test(data$question6, data$clicks, method = "pearson")
cor.test(data$question7, data$clicks, method = "pearson")
cor.test(data$question8, data$clicks, method = "pearson")
cor.test(data$question9, data$clicks, method = "pearson")
cor.test(data$question10, data$clicks, method = "pearson")
cor.test(data$question11, data$clicks, method = "pearson")
cor.test(data$question12, data$clicks, method = "pearson")
cor.test(data$question13, data$clicks, method = "pearson")
cor.test(data$question14, data$clicks, method = "pearson")

# Correlación con level
cor.test(data$question1, data$level, method = "pearson")
cor.test(data$question2, data$level, method = "pearson")
cor.test(data$question3, data$level, method = "pearson")
cor.test(data$question4, data$level, method = "pearson")
cor.test(data$question5, data$level, method = "pearson")
cor.test(data$question6, data$level, method = "pearson")
cor.test(data$question7, data$level, method = "pearson")
cor.test(data$question8, data$level, method = "pearson")
cor.test(data$question9, data$level, method = "pearson")
cor.test(data$question10, data$level, method = "pearson")
cor.test(data$question11, data$level, method = "pearson")
cor.test(data$question12, data$level, method = "pearson")
cor.test(data$question13, data$level, method = "pearson")
cor.test(data$question14, data$level, method = "pearson")

# Correlación con restarts
cor.test(data$question1, data$restarts, method = "pearson")
cor.test(data$question2, data$restarts, method = "pearson")
cor.test(data$question3, data$restarts, method = "pearson")
cor.test(data$question4, data$restarts, method = "pearson")
cor.test(data$question5, data$restarts, method = "pearson")
cor.test(data$question6, data$restarts, method = "pearson")
cor.test(data$question7, data$restarts, method = "pearson")
cor.test(data$question8, data$restarts, method = "pearson")
cor.test(data$question9, data$restarts, method = "pearson")
cor.test(data$question10, data$restarts, method = "pearson")
cor.test(data$question11, data$restarts, method = "pearson")
cor.test(data$question12, data$restarts, method = "pearson")
cor.test(data$question13, data$restarts, method = "pearson")
cor.test(data$question14, data$restarts, method = "pearson")

# Correlación con arrows_deleted
cor.test(data$question1, data$arrows_deleted, method = "pearson")
cor.test(data$question2, data$arrows_deleted, method = "pearson")
cor.test(data$question3, data$arrows_deleted, method = "pearson")
cor.test(data$question4, data$arrows_deleted, method = "pearson")
cor.test(data$question5, data$arrows_deleted, method = "pearson")
cor.test(data$question6, data$arrows_deleted, method = "pearson")
cor.test(data$question7, data$arrows_deleted, method = "pearson")
cor.test(data$question8, data$arrows_deleted, method = "pearson")
cor.test(data$question9, data$arrows_deleted, method = "pearson")
cor.test(data$question10, data$arrows_deleted, method = "pearson")
cor.test(data$question11, data$arrows_deleted, method = "pearson")
cor.test(data$question12, data$arrows_deleted, method = "pearson")
cor.test(data$question13, data$arrows_deleted, method = "pearson")
cor.test(data$question14, data$arrows_deleted, method = "pearson")

###########EXPONENCIAL###################################################
data <- form_tiles_exponencial
# Correlación con clicks
cor.test(data$question1, data$clicks, method = "pearson")
cor.test(data$question2, data$clicks, method = "pearson")
cor.test(data$question3, data$clicks, method = "pearson")
cor.test(data$question4, data$clicks, method = "pearson")
cor.test(data$question5, data$clicks, method = "pearson")
cor.test(data$question6, data$clicks, method = "pearson")
cor.test(data$question7, data$clicks, method = "pearson")
cor.test(data$question8, data$clicks, method = "pearson")
cor.test(data$question9, data$clicks, method = "pearson")
cor.test(data$question10, data$clicks, method = "pearson")
cor.test(data$question11, data$clicks, method = "pearson")
cor.test(data$question12, data$clicks, method = "pearson")
cor.test(data$question13, data$clicks, method = "pearson")
cor.test(data$question14, data$clicks, method = "pearson")

# Correlación con level
cor.test(data$question1, data$level, method = "pearson")
cor.test(data$question2, data$level, method = "pearson")
cor.test(data$question3, data$level, method = "pearson")
cor.test(data$question4, data$level, method = "pearson")
cor.test(data$question5, data$level, method = "pearson")
cor.test(data$question6, data$level, method = "pearson")
cor.test(data$question7, data$level, method = "pearson")
cor.test(data$question8, data$level, method = "pearson")
cor.test(data$question9, data$level, method = "pearson")
cor.test(data$question10, data$level, method = "pearson")
cor.test(data$question11, data$level, method = "pearson")
cor.test(data$question12, data$level, method = "pearson")
cor.test(data$question13, data$level, method = "pearson")
cor.test(data$question14, data$level, method = "pearson")

# Correlación con restarts
cor.test(data$question1, data$restarts, method = "pearson")
cor.test(data$question2, data$restarts, method = "pearson")
cor.test(data$question3, data$restarts, method = "pearson")
cor.test(data$question4, data$restarts, method = "pearson")
cor.test(data$question5, data$restarts, method = "pearson")
cor.test(data$question6, data$restarts, method = "pearson")
cor.test(data$question7, data$restarts, method = "pearson")
cor.test(data$question8, data$restarts, method = "pearson")
cor.test(data$question9, data$restarts, method = "pearson")
cor.test(data$question10, data$restarts, method = "pearson")
cor.test(data$question11, data$restarts, method = "pearson")
cor.test(data$question12, data$restarts, method = "pearson")
cor.test(data$question13, data$restarts, method = "pearson")
cor.test(data$question14, data$restarts, method = "pearson")

# Correlación con arrows_deleted
cor.test(data$question1, data$arrows_deleted, method = "pearson")
cor.test(data$question2, data$arrows_deleted, method = "pearson")
cor.test(data$question3, data$arrows_deleted, method = "pearson")
cor.test(data$question4, data$arrows_deleted, method = "pearson")
cor.test(data$question5, data$arrows_deleted, method = "pearson")
cor.test(data$question6, data$arrows_deleted, method = "pearson")
cor.test(data$question7, data$arrows_deleted, method = "pearson")
cor.test(data$question8, data$arrows_deleted, method = "pearson")
cor.test(data$question9, data$arrows_deleted, method = "pearson")
cor.test(data$question10, data$arrows_deleted, method = "pearson")
cor.test(data$question11, data$arrows_deleted, method = "pearson")
cor.test(data$question12, data$arrows_deleted, method = "pearson")
cor.test(data$question13, data$arrows_deleted, method = "pearson")
cor.test(data$question14, data$arrows_deleted, method = "pearson")
