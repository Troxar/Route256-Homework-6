## Домашнее задание на основе лекций 6 недели и воркшопа

Сервис предоставляет возможность поиска фильмов, шоу и интервью, где снимались указанные актеры.

### Что потребуется предварительно:
- Необходимо зарегистрировать свой API-ключ на [IMDb API](https://imdb-api.com). см. секцию `Steps to use the Services`
- Поднять в докере БД используя скрипт `init_db.sql`

### Основное задание:
- Отрефакторить код устранив эффект code smell и применив базовые техники рефакторинга и практические советы, изученные на [лекции](https://learning.ozon.ru/319/lp/442-route-256/6131-prodvinutaya-razrabotka-mikroservisov-na-c/video/25198-chistyy-kod-1608)
    - Доп. материал по техникам рефакторинга в книге `Refactoring. Improving the design of existing code. M. Fowler`
- Спроектировать луковую архитектуру
    - Выделить взаимодействие с IMDb API в отдельный компонент.
    - Каждый слой вынести в отдельный проект (опционально)
    - Исключить возможность излишней транзитивности между проектами (опционально)
- Реализовать логику для ведения справочника актеров в БД на основе результатов поиска

### Дополнительное задание: 
- Написать архитектурные тесты используя библиотеку [NetArchTest](https://github.com/BenMorris/NetArchTest)
