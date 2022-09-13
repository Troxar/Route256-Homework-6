create database "actorsdb";

create table if not exists actors
(
    actor_id bigint not null primary key generated always as identity,
    name varchar(255) not null,
    imdb_id varchar(9)
);

create index actors_name_idx on actors (name);
create index actors_imdb_id_idx on actors (imdb_id);

