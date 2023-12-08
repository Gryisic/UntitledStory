INCLUDE ../Functions.ink
INCLUDE ../TriggerIDs.ink

-> main

===main===
{~Очень [color_red]интересный[/color] [opacity_75]текст|А теперь магия пропала|Ээээмммм|Кек|Надеюсь, что тебе всё понравилось}. #s: Протагонист 
[color_orange]Надеюсь он будет удобно[/color] парсируемым.

    +[Повтор] -> first
    +[Не повтор] -> second
    +[Конец] -> third

===first===
Первый варик.
-> main

===second===
Второй варик.
~ DebugMessage("Tested")
-> DONE

===third===
~ ActivateTrigger(Test_0)
->END