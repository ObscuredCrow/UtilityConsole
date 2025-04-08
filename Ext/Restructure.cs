namespace UtilityConsole.Ext
{
    public static class Restructure
    {
        public static string Exclude = string.Empty;
        public static string FileType = string.Empty;
        public static string FirstSeparator = string.Empty;
        public static string SecondSeparator = string.Empty;

        public static void Process() {
            if (!Core.RunAgain) {
                Core.Path = string.Empty;
                Exclude = string.Empty;
                FileType = string.Empty;
                FirstSeparator = string.Empty;
                SecondSeparator = string.Empty;

                Console.WriteLine("What is the starting directory?");
                Core.Path = Console.ReadLine();

                Console.WriteLine("What part of the directory should be excluded?");
                Exclude = Console.ReadLine();
                if (Exclude == null)
                    Exclude = string.Empty;

                Console.WriteLine("What type of files are being changed?");
                FileType = "." + Console.ReadLine();

                Console.WriteLine("What's the first separator?");
                FirstSeparator = Console.ReadLine();

                Console.WriteLine("What's the second separator?");
                SecondSeparator = Console.ReadLine();

                Core.RunAgain = false;
            }

            if (Directory.Exists(Core.Path)) {
                foreach (var file in Directory.GetFiles(Core.Path, "*", SearchOption.AllDirectories)) {
                    FileInfo info = new FileInfo(file);
                    if (!info.Name.Contains(FileType)) {
                        Console.WriteLine($"Error - Ignoring: File Type Doesn't Match: {info.FullName.Replace(Exclude, "")}");
                        continue;
                    }

                    string newName = string.Empty;
                    if (string.IsNullOrEmpty(FirstSeparator) || (string.IsNullOrEmpty(FirstSeparator) && string.IsNullOrEmpty(SecondSeparator))) {
                        newName = "id";
                    }
                    else {
                        foreach (var dSplit in info.FullName.Split(FirstSeparator)) {
                            if (string.IsNullOrEmpty(SecondSeparator)) {
                                newName += dSplit[0];
                            }
                            else {
                                foreach (var lSplit in dSplit.Split(SecondSeparator)) {
                                    newName += lSplit[0];
                                }
                            }
                        }
                    }

                    newName.ToUpper();
                    string newPath = info.FullName.Replace(info.Name, $"{newName}{FileType}");

                    int count = 0;
                    while (File.Exists(newPath)) {
                        count++;
                        var anotherName = newName + count;
                        newPath = info.FullName.Replace(info.Name, $"{anotherName}{FileType}");
                    }

                    count = 0;
                    if (!File.Exists(newPath)) {
                        File.Move(info.FullName, newPath);
                        Console.WriteLine($"{info.FullName.Replace(Exclude, "")} => {newPath.Replace(Exclude, "").Replace(info.Name, $"{newName}")}");
                    }
                    else {
                        Console.WriteLine($"Error - Ignoring: New Path Already Exists: {info.FullName.Replace(Exclude, "")} => {newPath.Replace(Exclude, "").Replace(info.Name, $"{newName}")}");
                    }
                }

                Console.WriteLine($"Restructuring Complete!");
            }
            else {
                Console.WriteLine($"Not a valid path: {Core.Path}");
            }
        }
    }
}
