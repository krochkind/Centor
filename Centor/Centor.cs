using Centor.Enums;
using Centor.Extensions;
using Centor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Centor
{
	public static class Centor
	{
		[FunctionName("Centor")]
		[OpenApiOperation(operationId: "Centor",
			tags: new[] { "Centor" },
			Summary = "Determine via the Centor Criteria if there is strep",
			Description = "Centor",
			Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json",
			bodyType: typeof(SymptomModel),
			Required = true,
			Description = "The symptoms")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK,
			contentType: "application/json",
			bodyType: typeof(CentorResultModel),
			Summary = "The Centor Result file",
			Description = "The Centor Result")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest,
			Summary = "If the data is invalid",
			Description = "If the data is invalid")]
		[OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, 
			Name = "code", 
			In = OpenApiSecurityLocationType.Query)]

		public static async Task<IActionResult> Run(
					 [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
		{
			log.LogInformation("C# HTTP trigger function processed a request.");

			ValidationWrapper<SymptomModel> httpResponseBody = await req.GetBodyAsync<SymptomModel>();

			if (!httpResponseBody.IsValid)
			{
				log.LogInformation("Invalid request.");
				return new BadRequestObjectResult($"Invalid input: {string.Join(", ", httpResponseBody.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
			}

			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			var input = JsonConvert.DeserializeObject<SymptomModel>(requestBody);

			var centorScore = (int)input.Age;
			centorScore += input.SwolenTonsils ? 1 : 0;
			centorScore += input.SwolenLymphNodes ? 1 : 0;

			if (input.TemperatureFormat == TemperatureFormat.C || input.TemperatureFormat == TemperatureFormat.Celsius)
				centorScore += input.Temperature > 38 ? 1 : 0;
			else
				centorScore += input.Temperature > 100.4 ? 1 : 0;

			centorScore += input.Cough ? 0 : 1;

			var probability = centorScore switch
			{
				0 => new CentorResultModel { PctFrom = 1, PctTo = 2.5, Recommendation = "No further testing or antibiotics." },
				1 => new CentorResultModel { PctFrom = 5, PctTo = 10, Recommendation = "No further testing or antibiotics." },
				2 => new CentorResultModel { PctFrom = 11, PctTo = 17, Recommendation = "Optional rapid strep testing and/or culture." },
				3 => new CentorResultModel { PctFrom = 28, PctTo = 35, Recommendation = "Consider rapid strep testing and/or culture." },
				_ => new CentorResultModel { PctFrom = 51, PctTo = 53, Recommendation = "Consider rapid strep testing and/or culture. Empiric antibiotics may be appropriate depending on the specific scenario." },
			};

			log.LogInformation(JsonConvert.SerializeObject(probability));
			return new OkObjectResult(probability);
		}
	}
}
