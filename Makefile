MGR_NAME?=MyFirstMigration

setup:
	@dotnet restore

run:
	@dotnet watch

# create migration
cmgr:
	@dotnet ef migrations add $(MGR_NAME)

# apply migration
amgr:
	@dotnet ef database update

dc-up:
	@docker compose -p pbl6 up --build

dc-down:
	@docker compose -p pbl6 down