MGR_NAME?=MyFirstMigration

setup:
	@dotnet restore

run:
	@dotnet watch

# create migration
setup-mgr:
	@ln -snf ./../Migrations ./Migrations
	
cmgr:
	@dotnet ef migrations add $(MGR_NAME)

# apply migration
amgr:
	@dotnet ef database update

dc-up:
	@docker compose -p pbl6 up -d --build

dc-down:
	@docker compose -p pbl6 down

dc-clean:
	@docker system prune --all --force