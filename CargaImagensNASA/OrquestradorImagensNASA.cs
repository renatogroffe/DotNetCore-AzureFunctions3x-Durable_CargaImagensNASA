using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using CargaImagensNASA.Models;

namespace CargaImagensNASA
{
    public static class OrquestradorImagensNASA
    {
        [FunctionName("OrquestradorImagensNASA")]
        public static async Task<ResultadoExecucao> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var parametrosExecucao = context.GetInput<ParametrosExecucao>();

            var info = await context.CallActivityAsync<InfoImagemNASA>(
                "ObterDadosImagemPorData", parametrosExecucao);
            
            await context.CallActivityAsync("UploadImagemToStorage", info);

            return new ResultadoExecucao()
            {
                CodDataReferencia = parametrosExecucao.CodDataReferencia,
                Inicio = parametrosExecucao.Inicio,
                Termino = DateTime.Now
            };
        }
    }
}