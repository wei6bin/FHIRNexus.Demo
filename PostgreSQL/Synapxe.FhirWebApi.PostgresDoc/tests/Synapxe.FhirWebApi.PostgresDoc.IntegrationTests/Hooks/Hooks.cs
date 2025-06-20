﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Aspire.Hosting;
using Aspire.Hosting.Testing;
using Reqnroll;

namespace Synapxe.FhirWebApi.PostgresDoc.IntegrationTests;
[Binding]
public static class Hooks
{
    private static DistributedApplication application;

    [BeforeTestRun]
    public static async Task BeforeTestRun()
    {
        application = await AppFixture.CreateAppAsync<Projects.Synapxe_FhirWebApi_PostgresDoc_AppHost>();
    }

    [BeforeFeature]
    public static void BeforeFeature(FeatureContext featureContext)
    {
        featureContext.FeatureContainer.RegisterInstanceAs<Func<HttpClient>>(() => application.CreateHttpClient("server"));
    }

    [AfterTestRun]
    public static async Task AfterTestRun()
    {
        await application.DisposeAsync();
    }
}
