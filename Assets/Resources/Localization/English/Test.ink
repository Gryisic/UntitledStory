INCLUDE ../Functions.ink
INCLUDE ../TriggerIDs.ink

-> main

===main===
{~Very [color_red]interesting[/color] [opacity_75]text|Aw, magic diisapeared|Eeemmmmm|Lol|I hope, you like it}. #s: Protagonist 
[color_orange]I hope it'll be[/color] parseable.

    +[Repeat] -> first
    +[Repeat'nt] -> second
    +[End] -> third

===first===
First variant.
-> main

===second===
Second option.
~ DebugMessage("Tested")
-> DONE

===third===
~ ActivateTrigger(Test_0)
->END