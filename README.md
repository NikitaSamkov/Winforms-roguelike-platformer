Рогаликоподобный платформер, главной особенностью которого является то, что подбираемые предметы влияют не только на игрока (оружие, усилители), но и на игровой мир (изменение гравитации, врагов, и т.д.)
<br> С MVC мне помог Перов Юрий.
# Управление
W, Пробел - прыжок (вверх, если есть полёт)
<br> S - спрыгнуть с платформы (вниз, если есть полёт)
<br> A - влево
<br> D - вправо
<br> E - атака
<br> Q - выстрел
<br> 0 - перезапуск
<br> R - смена второго оружия (если возможно)
<br> ПКМ на сокровище в инвентаре - выбросить сокровище
<br> 
# Особенности
## Сокровища
На данный момент в игре есть 20 различных сокровищ:
![treasure 1](https://github.com/NikitaSamkov/Winforms-roguelike-platformer/blob/master/!Screenshots/for%20readme/treasures%201.png?raw=true)
![treasure 2](https://github.com/NikitaSamkov/Winforms-roguelike-platformer/blob/master/!Screenshots/for%20readme/treasures%202.png?raw=true)
![ruby](https://github.com/NikitaSamkov/Winforms-roguelike-platformer/blob/master/!Screenshots/for%20readme/giant%20ruby.png?raw=true)
<br>
## Сокровищница
Каждая третья комната в игре - сокровищница. По умолчанию (карта состоит из 10 комнат) на карте 3 сокровищницы.
<br> В сокровищнице находится одно из сокровищ на платформе. Враги в сокровищнице не появляются.
![treasure room 1](https://github.com/NikitaSamkov/Winforms-roguelike-platformer/blob/master/!Screenshots/for%20readme/treasure%20room%201.png?raw=true)
![treasure room 2](https://github.com/NikitaSamkov/Winforms-roguelike-platformer/blob/master/!Screenshots/for%20readme/treasure%20room%202.png?raw=true)
<br> 
## Враги
На данный момент в игре есть 15 типов врагов:
![enemy 1](https://github.com/NikitaSamkov/Winforms-roguelike-platformer/blob/master/!Screenshots/for%20readme/enemies%201.png?raw=true)
![enemy 2](https://github.com/NikitaSamkov/Winforms-roguelike-platformer/blob/master/!Screenshots/for%20readme/enemies%202.png?raw=true)
<br> 
## Конец игры
Для победы необходимо победить босса игры.
<br> Он всегда находится в последней комнате (десятой)
<br> При убийстве босса, с него всегда выпадает Giant Ruby
![boss 1](https://github.com/NikitaSamkov/Winforms-roguelike-platformer/blob/master/!Screenshots/for%20readme/boss%201.png?raw=true)
![boss 2](https://github.com/NikitaSamkov/Winforms-roguelike-platformer/blob/master/!Screenshots/for%20readme/boss%202.png?raw=true)
<br> У босса есть 2 атаки: удар кулаком, который наносит урон по области и призыв врагов (призывает 2 врагов: в левом верхнем углу и правом верхнем углу).
<br> Чем меньше здоровье босса - тем чаще он атакует.