using System.Text.Json;
using ZLogger;

namespace GameAPIServer;

public class SetLogger
{
	public static void SettingLogger(WebApplicationBuilder builder, IConfiguration configuration)
	{
        var logging = builder.Logging;

        logging.ClearProviders();

        var fileDir = configuration["logdir"];

        var exists = Directory.Exists(fileDir);

        if (exists == false)
        {
            Directory.CreateDirectory(fileDir);
        }

        logging.AddZLoggerRollingFile(
            (dt, x) => $"{fileDir}{dt.ToLocalTime():yyyy-MM-dd}_{x:000}.log",
            x => x.ToLocalTime().Date, 1024,
            options =>
            {
                options.EnableStructuredLogging = true;
                var time = JsonEncodedText.Encode("Timestamp");

                var timeValue = JsonEncodedText.Encode(DateTime.Now.AddHours(9).ToString("yyyy/MM/dd HH:mm:ss"));

                options.StructuredLoggingFormatter = (writer, info) =>
                {
                    writer.WriteString(time, timeValue);
                    info.WriteToJsonWriter(writer);
                };
            }); 

        logging.AddZLoggerConsole(options =>
        {
            options.EnableStructuredLogging = true;
            var time = JsonEncodedText.Encode("EventTime");
            var timeValue = JsonEncodedText.Encode(DateTime.Now.AddHours(9).ToString("yyyy/MM/dd HH:mm:ss"));

            options.StructuredLoggingFormatter = (writer, info) =>
            {
                writer.WriteString(time, timeValue);
                info.WriteToJsonWriter(writer);
            };
        });
    }
}