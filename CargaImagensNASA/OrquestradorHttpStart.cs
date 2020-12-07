using System;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using CargaImagensNASA.Models;

namespace CargaImagensNASA
{
    public static class OrquestradorHttpStart
    {
        [FunctionName("OrquestradorImagensNASA_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "imagemnasa/{dataref}")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            string dataref,
            ILogger log)
        {
            if (!Regex.IsMatch(dataref, @"^\d{4}-\d{2}-\d{2}$") ||
                !DateTime.TryParse(dataref, out var dataConvertida) ||
                dataConvertida > DateTime.Now || dataConvertida.Year < 2000)
            {
                log.LogError(
                    $"{nameof(HttpStart)} - " +
                    $"A data da imagem ({dataref}) deve estar no formato aaaa-mm-dd, " +
                    "ser menor ou igual à data atual e a partir do ano 2000!");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            log.LogInformation($"{nameof(HttpStart)} - Data-base informada = {dataref}");

            // Inicia a orquestração
            string instanceId = await starter.StartNewAsync<ParametrosExecucao>(
                "OrquestradorImagensNASA",
                new ParametrosExecucao()
                {
                    CodDataReferencia = dataref,
                    DataReferencia = dataConvertida
                });

            log.LogInformation($"{nameof(HttpStart)} - Iniciada orquestração com ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}