create table public.database_metainfo
(
   sigle_row bool primary key default true,
   database_version serial,
   service_version int[],
   constraint sigle_row_constraint check(sigle_row)
);

insert into database_metainfo values (true, 0, array[0, 0, 0]);