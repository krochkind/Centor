using Centor.Enums;
using System.ComponentModel.DataAnnotations;

namespace Centor.Models
{
	internal class SymptomModel
	{
		[Required]
		[EnumDataType(typeof(Age))]
		public Age Age { get; set; }
		[Required]
		public bool SwolenTonsils { get; set; }
		[Required]
		public bool SwolenLymphNodes { get; set; }
		[Required]
		[Range(30, 106)]
		public double Temperature { get; set; }
		[Required]
		[EnumDataType(typeof(TemperatureFormat))]
		public TemperatureFormat TemperatureFormat { get; set; } = TemperatureFormat.Fahrenheit;
		[Required]
		public bool Cough { get; set; }
	}
}
