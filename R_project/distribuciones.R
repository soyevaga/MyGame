# Space
space_lineal <- subset(space_last, difficulty == "Lineal")
space_exponencial <- subset(space_last, difficulty == "Exponencial")

shapiro.test(space_lineal$question3)
shapiro.test(space_lineal$question6)
shapiro.test(space_lineal$shoots)
shapiro.test(space_lineal$evens_killed)
shapiro.test(space_lineal$odds_killed)
shapiro.test(space_lineal$speed)
shapiro.test(space_lineal$self_killed)
shapiro.test(space_lineal$moving_time)

shapiro.test(space_exponencial$question3)
shapiro.test(space_exponencial$question6)
shapiro.test(space_exponencial$shoots)
shapiro.test(space_exponencial$evens_killed)
shapiro.test(space_exponencial$odds_killed)
shapiro.test(space_exponencial$speed)
shapiro.test(space_exponencial$self_killed)
shapiro.test(space_exponencial$moving_time)

# Cards
cards_lineal <- subset(cards_last, difficulty == "Lineal")
cards_exponencial <- subset(cards_last, difficulty == "Exponencial")

shapiro.test(cards_lineal$question3)
shapiro.test(cards_lineal$question6)
shapiro.test(cards_lineal$clicks)
shapiro.test(cards_lineal$level)
shapiro.test(cards_lineal$correct_pairs)
shapiro.test(cards_lineal$wrong_pairs)
shapiro.test(cards_lineal$double_checks)

shapiro.test(cards_exponencial$question3)
shapiro.test(cards_exponencial$question6)
shapiro.test(cards_exponencial$clicks)
shapiro.test(cards_exponencial$level)
shapiro.test(cards_exponencial$correct_pairs)
shapiro.test(cards_exponencial$wrong_pairs)
shapiro.test(cards_exponencial$double_checks)

# Tiles
tiles_lineal <- subset(tiles_last, difficulty == "Lineal")
tiles_exponencial <- subset(tiles_last, difficulty == "Exponencial")

shapiro.test(tiles_lineal$question3)
shapiro.test(tiles_lineal$question6)
shapiro.test(tiles_lineal$clicks)
shapiro.test(tiles_lineal$level)
shapiro.test(tiles_lineal$restarts)
shapiro.test(tiles_lineal$arrows_deleted)

shapiro.test(tiles_exponencial$question3)
shapiro.test(tiles_exponencial$question6)
shapiro.test(tiles_exponencial$clicks)
shapiro.test(tiles_exponencial$level)
shapiro.test(tiles_exponencial$restarts)
shapiro.test(tiles_exponencial$arrows_deleted)
