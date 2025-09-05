postgres-up:
	docker run --name taskly-local-db \
		-e POSTGRES_PASSWORD=randompassword \
		-e POSTGRES_DB=Taskly \
		-p 5432:5432 \
		-d postgres:17

postgres-down:
	docker rm -f taskly-local-db

add-migration:
	 dotnet ef migrations add $(name) \
        --project src/Taskly.Infrastructure \
        --startup-project src/Taskly.API

update-db:
	 dotnet ef database update \
		--project src/Taskly.Infrastructure \
		--startup-project src/Taskly.API

run-app:
	dotnet run --project Taskly.API

build-debug:
    dotnet build Taskly.API/Taskly.API.csproj -c Debug

build-release:
    dotnet build Taskly.API/Taskly.API.csproj -c Release
