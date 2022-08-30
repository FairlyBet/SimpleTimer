namespace SimpleTimer
{
    internal static class TimerFactory
    {
        private const string _filePath = "config.txt";
        private static int _minutesForWork;
        private static int _minutesForRest;


        static TimerFactory()
        {
            ConfigureFromFile();
        }

        public static AsyncSecondBasedTimer TimerForWork()
        {
            return new(60 * _minutesForWork);
        }

        public static AsyncSecondBasedTimer TimerForRest()
        {
            return new(60 * _minutesForRest);
        }

        private static void ConfigureFromFile()
        {
            EnsureFileExistance();
            StreamReader streamReader = new(_filePath);
            if (!int.TryParse(streamReader.ReadLine(), out _minutesForWork)
                || !int.TryParse(streamReader.ReadLine(), out _minutesForRest)
                || _minutesForWork < 0 || _minutesForRest < 0)
            {
                MessageBox.Show("Invalid parameters in config.txt!\nParameters set to default");
                File.Delete(_filePath);
                EnsureFileExistance();
            }
        }

        private static void EnsureFileExistance()
        {
            FileInfo fileInfo = new(_filePath);
            if (!fileInfo.Exists)
            {
                using var fileWriter = File.CreateText(_filePath);
                fileWriter.WriteLine(30);
                fileWriter.WriteLine(5);
                fileWriter.Close();
            }
        }
    }
}
