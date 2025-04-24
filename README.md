# Hanoi tornyai

Azt tartja a legenda, hogy valahol Ázsiában *(Tibetben, Vietnámban vagy Indiában – döntsd el, melyik internetes oldal legendája tetszik a legjobban)* szerzetesek ezt a problémát próbálják megoldani 64 koronggal, és – a legenda szerint – a szerzetesek úgy tartják, hogy amikor sikerül mind a 64 korongot átrakni az A rúdról a B rúdra a fenti szabályok betartásával, akkor a világnak vége szakad. Ha a szerzeteseknek igaza van, van-e okunk pánikba esni?

A feladat kiinduló helyzetében adott egy torony, amely különböző méretű korongokból áll, és ezeket egy oszlopra helyeztük úgy, hogy a nagyobb korongok mindig a kisebb korongok alatt vannak. A cél, hogy a tornyot egy másik oszlopra helyezzük át úgy, hogy betartjuk a következő szabályokat:

- Egyszerre csak egy korongot mozgathatunk.
- Egy korongot sosem helyezhetünk egy kisebb korongra.
- A tornyot egy harmadik segédoszlop segítségével kell áthelyeznünk.

Ha `n` a korongok száma, akkor az optimális megoldáshoz szükséges lépések száma: `2^n - 1`.  
*(A képlet jelentése: 2 az n-edik hatványon mínusz 1.)*