library(dplyr)

cards <- read.csv("cards.csv", header = TRUE, sep = ",", encoding = "UTF-8")
form <- read.csv("form.csv", header = TRUE, sep = ",", encoding = "UTF-8")

# Juntar las tablas
form_cards <- form %>%
  filter(game == "cards") 

form_cards <- cards %>%
  left_join(form_cards, by = "userID") %>%
  filter(!is.na(question3))

# Quedarse solo con la última repetición de cada usuario
form_cards <- form_cards %>%
  group_by(userID) %>%
  slice_max(order_by = time, n = 1, with_ties = FALSE) %>%
  ungroup()

form_cards_lineal <- subset(form_cards, difficulty == "Lineal")
form_cards_exponencial <- subset(form_cards, difficulty == "Exponencial")

###################################################################
data <- form_cards_lineal
data <- form_cards_exponencial

cor.test(data$question3, data$clicks, method = "pearson")
cor.test(data$question6, data$clicks, method = "pearson")
cor.test(data$question3, data$level, method = "pearson")
cor.test(data$question6, data$level, method = "pearson")
cor.test(data$question3, data$correct_pairs, method = "pearson")
cor.test(data$question6, data$correct_pairs, method = "pearson")
cor.test(data$question3, data$wrong_pairs, method = "pearson")
cor.test(data$question6, data$wrong_pairs, method = "pearson")
cor.test(data$question3, data$double_checks, method = "pearson")
cor.test(data$question6, data$double_checks, method = "pearson")

cor.test(data$question3, data$clicks, method = "spearman")
cor.test(data$question6, data$clicks, method = "spearman")
cor.test(data$question3, data$level, method = "spearman")
cor.test(data$question6, data$level, method = "spearman")
cor.test(data$question3, data$correct_pairs, method = "spearman")
cor.test(data$question6, data$correct_pairs, method = "spearman")
cor.test(data$question3, data$wrong_pairs, method = "spearman")
cor.test(data$question6, data$wrong_pairs, method = "spearman")
cor.test(data$question3, data$double_checks, method = "spearman")
cor.test(data$question6, data$double_checks, method = "spearman")

###########LINEAL###################################################
data <- form_cards_lineal
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

# Correlación con correct_pairs
cor.test(data$question1, data$correct_pairs, method = "pearson")
cor.test(data$question2, data$correct_pairs, method = "pearson")
cor.test(data$question3, data$correct_pairs, method = "pearson")
cor.test(data$question4, data$correct_pairs, method = "pearson")
cor.test(data$question5, data$correct_pairs, method = "pearson")
cor.test(data$question6, data$correct_pairs, method = "pearson")
cor.test(data$question7, data$correct_pairs, method = "pearson")
cor.test(data$question8, data$correct_pairs, method = "pearson")
cor.test(data$question9, data$correct_pairs, method = "pearson")
cor.test(data$question10, data$correct_pairs, method = "pearson")
cor.test(data$question11, data$correct_pairs, method = "pearson")
cor.test(data$question12, data$correct_pairs, method = "pearson")
cor.test(data$question13, data$correct_pairs, method = "pearson")
cor.test(data$question14, data$correct_pairs, method = "pearson")

# Correlación con wrong_pairs
cor.test(data$question1, data$wrong_pairs, method = "pearson")
cor.test(data$question2, data$wrong_pairs, method = "pearson")
cor.test(data$question3, data$wrong_pairs, method = "pearson")
cor.test(data$question4, data$wrong_pairs, method = "pearson")
cor.test(data$question5, data$wrong_pairs, method = "pearson")
cor.test(data$question6, data$wrong_pairs, method = "pearson")
cor.test(data$question7, data$wrong_pairs, method = "pearson")
cor.test(data$question8, data$wrong_pairs, method = "pearson")
cor.test(data$question9, data$wrong_pairs, method = "pearson")
cor.test(data$question10, data$wrong_pairs, method = "pearson")
cor.test(data$question11, data$wrong_pairs, method = "pearson")
cor.test(data$question12, data$wrong_pairs, method = "pearson")
cor.test(data$question13, data$wrong_pairs, method = "pearson")
cor.test(data$question14, data$wrong_pairs, method = "pearson")

# Correlación con double_checks
cor.test(data$question1, data$double_checks, method = "pearson")
cor.test(data$question2, data$double_checks, method = "pearson")
cor.test(data$question3, data$double_checks, method = "pearson")
cor.test(data$question4, data$double_checks, method = "pearson")
cor.test(data$question5, data$double_checks, method = "pearson")
cor.test(data$question6, data$double_checks, method = "pearson")
cor.test(data$question7, data$double_checks, method = "pearson")
cor.test(data$question8, data$double_checks, method = "pearson")
cor.test(data$question9, data$double_checks, method = "pearson")
cor.test(data$question10, data$double_checks, method = "pearson")
cor.test(data$question11, data$double_checks, method = "pearson")
cor.test(data$question12, data$double_checks, method = "pearson")
cor.test(data$question13, data$double_checks, method = "pearson")
cor.test(data$question14, data$double_checks, method = "pearson")

###########EXPONENCIAL###################################################
data <- form_cards_exponencial
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

# Correlación con correct_pairs
cor.test(data$question1, data$correct_pairs, method = "pearson")
cor.test(data$question2, data$correct_pairs, method = "pearson")
cor.test(data$question3, data$correct_pairs, method = "pearson")
cor.test(data$question4, data$correct_pairs, method = "pearson")
cor.test(data$question5, data$correct_pairs, method = "pearson")
cor.test(data$question6, data$correct_pairs, method = "pearson")
cor.test(data$question7, data$correct_pairs, method = "pearson")
cor.test(data$question8, data$correct_pairs, method = "pearson")
cor.test(data$question9, data$correct_pairs, method = "pearson")
cor.test(data$question10, data$correct_pairs, method = "pearson")
cor.test(data$question11, data$correct_pairs, method = "pearson")
cor.test(data$question12, data$correct_pairs, method = "pearson")
cor.test(data$question13, data$correct_pairs, method = "pearson")
cor.test(data$question14, data$correct_pairs, method = "pearson")

# Correlación con wrong_pairs
cor.test(data$question1, data$wrong_pairs, method = "pearson")
cor.test(data$question2, data$wrong_pairs, method = "pearson")
cor.test(data$question3, data$wrong_pairs, method = "pearson")
cor.test(data$question4, data$wrong_pairs, method = "pearson")
cor.test(data$question5, data$wrong_pairs, method = "pearson")
cor.test(data$question6, data$wrong_pairs, method = "pearson")
cor.test(data$question7, data$wrong_pairs, method = "pearson")
cor.test(data$question8, data$wrong_pairs, method = "pearson")
cor.test(data$question9, data$wrong_pairs, method = "pearson")
cor.test(data$question10, data$wrong_pairs, method = "pearson")
cor.test(data$question11, data$wrong_pairs, method = "pearson")
cor.test(data$question12, data$wrong_pairs, method = "pearson")
cor.test(data$question13, data$wrong_pairs, method = "pearson")
cor.test(data$question14, data$wrong_pairs, method = "pearson")

# Correlación con double_checks
cor.test(data$question1, data$double_checks, method = "pearson")
cor.test(data$question2, data$double_checks, method = "pearson")
cor.test(data$question3, data$double_checks, method = "pearson")
cor.test(data$question4, data$double_checks, method = "pearson")
cor.test(data$question5, data$double_checks, method = "pearson")
cor.test(data$question6, data$double_checks, method = "pearson")
cor.test(data$question7, data$double_checks, method = "pearson")
cor.test(data$question8, data$double_checks, method = "pearson")
cor.test(data$question9, data$double_checks, method = "pearson")
cor.test(data$question10, data$double_checks, method = "pearson")
cor.test(data$question11, data$double_checks, method = "pearson")
cor.test(data$question12, data$double_checks, method = "pearson")
cor.test(data$question13, data$double_checks, method = "pearson")
cor.test(data$question14, data$double_checks, method = "pearson")

