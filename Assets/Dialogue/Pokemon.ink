INCLUDE global.ink

{ pokemon_name == "" : -> main | -> already_chose}

=== main ===
Hello big Bob
Hi little Bob
Which pokemon do you choose?
+[Charmander]
   -> chosen("Charmander")
+[Bulbasaur]
    -> chosen("Bulbasaur")
+[Squirtle]
-> chosen("Squirtle")

===chosen(pokemon)===
~ pokemon_name = pokemon
You chose {pokemon}!
->END

=== already_chose ===
You already chose {pokemon_name}!
-> END