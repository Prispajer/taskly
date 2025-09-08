postgres-up:
	docker run --name taskly-local-db \
		-e POSTGRES_PASSWORD=randompassword \
		-e POSTGRES_DB=Taskly \
		-p 5432:5432 \
		-d postgres:17

postgres-down:
	docker rm -f taskly-local-db

add-migration:
	dotnet ef migrations add "$(name)" \
		--project src/Taskly.Infrastructure \
		--startup-project src/Taskly.API

update-db:
	dotnet ef database update \
		--project src/Taskly.Infrastructure \
		--startup-project src/Taskly.API

run-app:
	dotnet run --project src/Taskly.API

build:
	dotnet build src/Taskly.API/Taskly.API.csproj -c Release

test:
	dotnet test tests/Taskly.IntegrationTests/Taskly.IntegrationTests.csproj
	dotnet test tests/Taskly.UnitTests/Taskly.Uni	tTests.csproj

docker-build:
	docker build -t taskly-api .

docker-run:
	docker run -p 5000:8080 taskly-api 

docker-compose-up:
	docker-compose up --build 

docker-compose-down:
	docker-compose down -v

clean:
	dotnet clean src/Taskly.API/Taskly.API.csproj
