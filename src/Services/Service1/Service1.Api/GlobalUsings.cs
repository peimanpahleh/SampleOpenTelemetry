global using EventBus.Abstractions;
global using Microsoft.Extensions.Logging;
global using System.Net;
global using EventBus;
global using MassTransit;
global using EventBus.Events;
global using EventBus.IntegrationEvents.Service1;
global using Serilog;
global using Service1.Api;
global using Serilog.Sinks.SystemConsole.Themes;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using MassTransit.PrometheusIntegration;
global using OpenTelemetry.Resources;
global using OpenTelemetry.Trace;
global using Prometheus;
global using Serilog.Enrichers.Span;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Service1.Api.Data.Models;
global using Service1.Api.Data;
global using Npgsql;
global using static Service2.Api.Greeter;






