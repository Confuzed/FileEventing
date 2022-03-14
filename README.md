# File Eventing

A POC project demonstrating a monitoring process raising events using MassTransit over RabbitMQ which can then be consumed by a server-side process.

## Build and Test

The easiest way to build and test the whole solution is using the docker compose file, in the solution root.

The docker compose file will build both the Monitor and the Service components, before starting them alongside a Rabbit MQ instance which they use for communicating events.

e.g.

```bash
$ docker-compose up --build -d
```


## To Do...

* [ ] Set up a launch profile that utilises the docker compose file so that it can be used from editors such as Rider and VS Code.
