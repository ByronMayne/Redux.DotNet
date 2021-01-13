using System;
using System.Text.RegularExpressions;

namespace ReduxSharp.Logging
{

    internal class DefaultLogger : ILog
    {
        private readonly Regex m_matcher;
        private readonly LoggerOptions m_options;

        public DefaultLogger(LoggerOptions options)
        {
            m_options = options;
            m_matcher = new Regex(@"\{.*\}");
        }

        public void Error(string messageTemplate, params object[] parameters)
            => WriteLog(LogLevel.Error, messageTemplate, parameters);

        public void Fatal(string messageTemplate, params object[] parameters)
            => WriteLog(LogLevel.Fatal, messageTemplate, parameters);

        public void Information(string messageTemplate, params object[] parameters)
            => WriteLog(LogLevel.Information, messageTemplate, parameters);

        public void Verbose(string messageTemplate, params object[] parameters)
            => WriteLog(LogLevel.Verbose, messageTemplate, parameters);

        public void Warning(string messageTemplate, params object[] parameters)
            => WriteLog(LogLevel.Warning, messageTemplate, parameters);


        private void WriteLog(LogLevel level, string message, params object[] parameters)
        {
            if(m_options.MinimumLogLevel > level)
            {
                return;
            }

            MatchCollection matches = m_matcher.Matches(message);

            for(int i = 0; i < matches.Count && i < parameters.Length; i++)
            {
                message = matches[i].Result(parameters[i].ToString());
            }

            Console.Write($"[{DateTime.Now:HH:mm:ss} ");
            switch (level)
            {
                case LogLevel.Error:
                case LogLevel.Fatal:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Information:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
            }
            Console.Write($"{level:xxx}");
            Console.ResetColor();

            Console.WriteLine(message);
        }

        private string Evaluator(Match match)
        {
            throw new NotImplementedException();
        }
    }
}
