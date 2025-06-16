
form <- read.csv("form.csv", header = TRUE, sep = ",", encoding = "UTF-8")
variables <- form[, c("question1", "question2", "question3","question4", 
                       "question5", "question6","question7", "question8", 
                       "question9", "question10", "question11", "question12",
                       "question13", "question14")]

# Calcular la matriz de covarianzas
cov_matrix <- cov(variables)
View(cov_matrix)
# Cambiar nombres de filas y columnas
colnames(cov_matrix) <- rownames(cov_matrix) <- c("Q1", "Q2", "Q3", "Q4", "Q5",
                                                  "Q6", "Q7", "Q8", "Q9", "Q10",
                                                  "Q11", "Q12", "Q13", "Q14")

# Dibujar el heatmap
image(1:14, 1:14, t(apply(cov_matrix, 2, rev)), 
      col = colorRampPalette(c("white", "skyblue", "blue"))(100), 
      axes = FALSE, xlab = "", ylab = "", main = "Matriz de Covarianzas")

# Añadir nombres de variables en los ejes
axis(1, at = 1:14, labels = colnames(cov_matrix))
axis(2, at = 1:14, labels = rev(rownames(cov_matrix)))

# Añadir los valores redondeados en las celdas
for (i in 1:14) {
  for (j in 1:14) {
    text(i, j, round(cov_matrix[15 - j, i], 2))
  }
}

table(form$question11, form$question14)

