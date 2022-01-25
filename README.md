SampleTelemetry

## This is just a sample project for testing OpenTelemetry in dotnet

 i used this stacks for this sample project:
- .net6
- opentelemetry
- postgres
- rabbitmq
- masstransit
- grpc
- zipkin
- promethues
- graphana
- serilog
- seq

i have 2 apis named Service1 and Service2

Service1 Controllers:

api/Test1

this controller sends a request to Service2 via rabbitmq using masstransit

