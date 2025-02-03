do
$remove_role$
begin
   if not exists (
      select from pg_catalog.pg_roles
      where  rolname = 'tt_test_role') then 

      raise notice 'failed to delete role "tt_test_role", role does not exists. Skipping.';
   else
      drop role tt_test_role;
      raise notice 'role "tt_test_role" successfully deleted';
   end if;
end
$remove_role$;