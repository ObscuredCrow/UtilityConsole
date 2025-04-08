namespace UtilityConsole.Ext
{
    public static class Rename
    {
        public static string Replace = string.Empty;
        public static string With = string.Empty;

        public static void Process() {
            if (!Core.RunAgain) {
                Core.Path = string.Empty;

                Console.WriteLine("What is the starting directory?");
                Core.Path = Console.ReadLine();

                Console.WriteLine("What part of the name do you wish to replace?");
                Replace = Console.ReadLine();

                Console.WriteLine("What would you like to replace it with?");
                With = Console.ReadLine();

                Core.RunAgain = false;
            }

            if(!string.IsNullOrEmpty(Replace)) {
                if (Directory.Exists(Core.Path)) {
                    foreach (var file in Directory.GetFiles(Core.Path, "*", SearchOption.AllDirectories)) {
                        FileInfo info = new FileInfo(file);

                        string newName = info.Name.Replace(Replace, With);
                        string newPath = info.FullName.Replace(info.Name, newName);

                        if (!File.Exists(newPath)) {
                            File.Move(info.FullName, newPath);
                            Console.WriteLine($"{info.FullName.Replace(Core.Path, "")} => {newPath.Replace(Core.Path, "").Replace(info.Name, $"{newName}")}");
                        }
                        else {
                            Console.WriteLine($"Error - Ignoring: New Path Already Exists: {info.FullName.Replace(Core.Path, "")} => {newPath.Replace(Core.Path, "").Replace(info.Name, $"{newName}")}");
                        }
                    }

                    Console.WriteLine($"Restructuring Complete!");
                }
                else {
                    Console.WriteLine($"Not a valid path: {Core.Path}");
                }
            }
            else {
                Console.WriteLine($"No replacement supplied, aborting.");
            }
        }
    }
}
