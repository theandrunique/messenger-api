APP=docker-compose.yml

ENV_FILE = --env-file ./.env.docker

APP_SERVICE=app

.PHONY: up
up:
	docker compose ${ENV_FILE} -f ${APP} up --build -d

.PHONY: down
down:
	docker compose ${ENV_FILE} -f ${APP} down

.PHONY: shell
shell:
	docker compose ${ENV_FILE} -f ${APP} exec ${APP_SERVICE} bash

.PHONY: logs
logs:
	docker compose ${ENV_FILE} -f ${APP} logs ${APP_SERVICE} -f

.PHONY: migrations
migrations:
	dotnet ef database update -s MessengerAPI.Presentation -p MessengerAPI.Infrastructure

