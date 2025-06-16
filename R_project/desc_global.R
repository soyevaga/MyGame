######Descriptivo global########
data <- read.csv("users.csv", header = TRUE, sep = ",", encoding = "UTF-8")
data <- data[data$age >= 10 & data$age <= 90, ]
summary(data)

sd(data$age, na.rm = TRUE)
sd(data$hours, na.rm = TRUE)

data <- read.csv("users.csv", header = TRUE, sep = ",", encoding = "UTF-8")
barplot(table(data$gender), main="Participantes por género", ylab="Número de participantes", col="turquoise3")
barplot(table(data$age), main="Participantes por edad", ylab="Número de participantes", col="salmon")

t.test(age ~ birthday, data = data)
fisher.test(table(data$birthday, data$gender))

table(data$gender)
table(data$birthday)
tabla <- table(data$birthday, data$gender)

prop.table(tabla, margin = 1)
barplot(prop.table(tabla, margin = 1), beside = TRUE, legend = TRUE, col = c("turquoise3", "salmon"))

######Descriptivo por grupo######
barplot(table(data$birthday), main="Participantes por grupo", ylab="Número de participantes", col="chartreuse")
