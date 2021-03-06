global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;
global using EventBus.Abstractions;
global using EventBus.IntegrationEvents.Prodcuts;
global using MassTransit;
global using Baskets.Api.EventsHandler;
global using Baskets.Api.Settings;
global using EventBus;
global using EventBus.Configurations;
global using Microsoft.Extensions.Logging;
global using Baskets.Api;
global using Serilog;
global using Serilog.Enrichers.Span;
global using MassTransit.PrometheusIntegration;
global using Prometheus;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Npgsql;
global using OpenTelemetry.Resources;
global using OpenTelemetry.Trace;



