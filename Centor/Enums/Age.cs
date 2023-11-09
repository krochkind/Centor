using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Centor.Enums
{
	[JsonConverter(typeof(StringEnumConverter))]
	enum Age
	{
		Under3 = -2,
		Age3to14 = 1,
		Age15to44 = 0,
		Age45plus = -1
	}
}
