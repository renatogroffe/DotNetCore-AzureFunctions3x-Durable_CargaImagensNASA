using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Refit;
using CargaImagensNASA.Models;
using CargaImagensNASA.HttpClients;

namespace CargaImagensNASA
{
    public static class FunctionObterDadosImagemPorData
    {
        [FunctionName("ObterDadosImagemPorData")]
        public static async Task<InfoImagemNASA> ObterDadosImagemPorData(
            [ActivityTrigger] ParametrosExecucao parametrosExecucao,
            ILogger log)
        {
            log.LogInformation(
                $"{nameof(ObterDadosImagemPorData)} - Iniciando a execução...");
            
            var imagemDiariaAPI = RestService.For<IImagemDiariaAPI>(
                Environment.GetEnvironmentVariable("EndpointNASA"));
            var infoImagemNASA = await imagemDiariaAPI.GetInfoAsync(
                Environment.GetEnvironmentVariable("APIKeyNASA"),
                parametrosExecucao.CodDataReferencia);

            log.LogInformation(
                $"{nameof(ObterDadosImagemPorData)} - Dados retornados pela API: " +
                JsonSerializer.Serialize(infoImagemNASA));

            return infoImagemNASA;
        }
    }
}