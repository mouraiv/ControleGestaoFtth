using GroupDocs.Conversion;
using GroupDocs.Conversion.Options.Convert;
using GroupDocs.Conversion.Reporting;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ControleGestaoFtth.ComponentModel
{
    public class ConversionViewModel : IConverterListener
    {
        public string InputFilePath { get; set; } = null!;
        public byte[] OutputFilePath { get; set; } = null!;
        public bool IsConversionComplete { get; set; }
        public int Progresso { get; set; }

        public void ConvertFileInBackground()
        {

            ConverterSettings config = new()
            {
                Listener = this
            };

            var conversionOptions = new PdfConvertOptions();

            conversionOptions.PdfOptions.DocumentInfo.Title = Regex.Match(InputFilePath, @"[^\\/:*?""<>|\r\n]+$").Value;

            Converter converter = new(InputFilePath, () => config);

            MemoryStream output = new();

            converter.Convert(() => output, conversionOptions);

            OutputFilePath = output.ToArray();

            converter.Dispose();

        }
        public void Completed()
        {
            IsConversionComplete = true;
        }
        public void Progress(byte current)
        {
            Progresso = current;
        }
        public void Started()
        {
            IsConversionComplete = false;
        }
    }
}
