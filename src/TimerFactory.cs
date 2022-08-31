namespace SimpleTimer
{
    internal static class TimerFactory
    {
        private const string FilePath = "config.txt";
        private const int DefaultWorkMinutes = 30;
        private const int DefaultRestMinutes = 5;
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
            StreamReader streamReader = new(FilePath);
            if (!int.TryParse(streamReader.ReadLine(), out _minutesForWork)
                || !int.TryParse(streamReader.ReadLine(), out _minutesForRest)
                || _minutesForWork < 0 || _minutesForRest < 0)
            {
                MessageBox.Show("Invalid parameters in config.txt!\nParameters set to default");
                streamReader.Close();
                File.Delete(FilePath);
                EnsureFileExistance();
                _minutesForWork = DefaultWorkMinutes;
                _minutesForRest = DefaultRestMinutes;
            }
        }

        private static void EnsureFileExistance()
        {
            FileInfo fileInfo = new(FilePath);
            if (!fileInfo.Exists)
            {
                using var fileWriter = File.CreateText(FilePath);
                fileWriter.WriteLine(DefaultWorkMinutes);
                fileWriter.WriteLine(DefaultRestMinutes);
                fileWriter.Close();
            }
        }
    }
}
