// USAGE:
//   /api/dreidel-spinner                  return the symbol
//   /api/dreidel-spinner?words=[true|1]   return the words

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TwentyFiveDaysOfServerless
{
    public static class DreidelSpinner
    {
        private const int SymbolCount = 4;
        private static readonly string[] Symbols = new string[SymbolCount] { "נ", "ג", "ה", "ש" };
        private static readonly string[] Words = new string[SymbolCount] { "Nun", "Gimmel", "Hay", "Shin" };

        [FunctionName("dreidel-spinner")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            // logging, because it needs to be done
            log.LogInformation("Spinning the dreidel!");

            // get the desired type of response
            var symbolArray = Symbols;
            if (req.Query.TryGetValue("words", out var v) && (v.ToString().ToLowerInvariant() == "true" || v == "1"))
                symbolArray = Words;

            // get the random side
            var random = new Random();
            var randomSide = random.Next(SymbolCount);

            // get the correct word or symbol
            var side = symbolArray[randomSide];

            // send it back
            return new OkObjectResult(side);
        }
    }
}
