INCLUDE global.ink

VAR ActiveSelf = true
// ~ ActiveSelf = TalkedWithMagnate

=== mainRefugeeMainRoom ===
// Проверка условия без использования return
{ 
    - ActiveSelf:
        <i>В голове мелькает: как же эти раздолбаи мне уже надоели, встань и сделай хоть что-то</i>
        #speaker:Петро 
        #portrait:ms_yellow_neutral 
        #layout:left
        -> mainRefugeeMainRoom_continue
    - else:
        // Можно либо ничего не делать, либо перейти в другой узел
        -> mainRefugeeMainRoom_continue
}

=== mainRefugeeMainRoom_continue ===
"Слушай, хватит уже болтать ерунду. Нам нужно готовиться к битве и решать насущные проблемы, а не устроить тут балаган. Давай спокойно."
#speaker:Петро 
#portrait:ms_yellow_neutral 
#layout:left

(Петро останавливается в полушаге от бродяги...)

* [Показать кулон]
    -> gate1RefugeeMainRoom
* [Проявить агрессию]
    -> gate2RefugeeMainRoom

=== gate1RefugeeMainRoom ===
"Видишь это? Мою семью убили так же..."
#speaker:Петро 
#portrait:ms_yellow_neutral 
#layout:left
-> END

=== gate2RefugeeMainRoom ===
"Твои слёзы мне противны..."
#speaker:Петро 
#portrait:ms_yellow_neutral 
#layout:left
~ PowerCheckStart = true
-> END