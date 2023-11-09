using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Centor.Enums
{
	[JsonConverter(typeof(StringEnumConverter))]
	enum TemperatureFormat
	{
		Fahrenheit,
		F,
		Celsius,
		C
	}
}
