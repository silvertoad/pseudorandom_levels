2d world generated example
=========

Исходная задача:

- Танки с бесконечной картой. Игрок управляет танком. Вид строго сверху (2D). 

- Мир бесконечный, в нем есть четыре вида объектов: деревья, кусты, камни, лужи. Кол-во объектов на экране - 50, относительная вероятность появления объектов: деревья - 10%, кусты - 30%, лужи - 10%, камни - 50%. Игрок может ехать сколь угодно долго, он не упрется в край мира. 

- Мир запоминается, то есть, если игрок в другой сессии попадает в ту же точку, он видит тот же пейзаж (то же расположение объектов).
Управление танком - влево/вправо - поворот танка, вверх/вниз - ехать вперед/назад. 

- Раз в 5 секунд на карте генерируется куст случайным образом (вероятность появления тем больше, чем меньше кустов рядом).


Создание мира
=========

Мир разбит на регионы. Каждый регион генерируется посредством ГПСЧ на основе сид стэйта и индекса региона. Игра стартует в индексе региона **{ 0;0 }**. Стэйт сид каждого региона генерируется в соответствии с алгоритмом:
> seed_state + region_index = region_seed_state

Создание кустов осуществляется через основной алгоритм генерации контента региона с использованием ГПСЧ.

Сериализация мира
=========

Так как генерация мира детерминирована для восстановления состояния любого региона необходимо хранить только его индекс и стэйт сид.
Однако так как индекс региона в который добавляется куст и позиция танка в регионе определяются действиями пользователя, необходимо хранить дополнительную информацию: позицию танка в регионе на момент сохранения и количество добавленных кустов в регионе.

На диске состояние мира хранится в следующей структуре:

``` js
{
	"seed" : 546724,
	"current_region" : "-1;2",
	"player_pos" : "2;5",
	"bushs" : {
		"0;1" : 1,
		"-1;2" : 2
	}
}
```

Онлайн
=========

Так как игроки не могут взаимодействовать друг с другом для их отображения каждому из клиентов будет достаточно броадкаcтить координаты других игроков. Однако генерацию кустов (расчет индекса и времени создания) необходимо будет производить на сервере исходя из позиций игроков на карте и единого серверного времени.

Другие пояснения
=========

- Для передвижения используйте клавиши **WASD**.
- Целевая версия **Unity Editor 4.5.2+**
- Сборка не рассчитана на использование в **Unity WebPlayer**, так как использует сохранение в файл.