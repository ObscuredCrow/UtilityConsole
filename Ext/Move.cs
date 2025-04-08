using System.IO;

namespace UtilityConsole.Ext
{
    public static class Move
    {
        public static string tags = string.Empty;
        public static string subTags = string.Empty;

        public static void Process() {
            if (!Core.RunAgain) {
                Core.Path = string.Empty;
                tags = string.Empty;
                subTags = string.Empty;

                Console.WriteLine("What is the starting directory?");
                Core.Path = Console.ReadLine();

                Console.WriteLine("What are the directory tags?");
                tags = Console.ReadLine();

                Console.WriteLine("What are the directory sub tags?");
                subTags = Console.ReadLine();

                Core.RunAgain = false;
            }

            if (Directory.Exists(Core.Path)) {
                List<string[]> filesToMove = new List<string[]>();
                foreach (var file in Directory.GetFiles(Core.Path, "*", SearchOption.AllDirectories)) {
                    FileInfo info = new FileInfo(file);

                    string origPath = info.FullName;
                    string newPath = string.Empty;
                    if (string.IsNullOrEmpty(tags))
                        break;

                    foreach (var tag in tags.Split(';')) {
                        if (origPath.Contains(tag)) {
                            bool foundSubTag = false;
                            if (!string.IsNullOrEmpty(subTags)) {
                                foreach (var subTag in subTags.Split(';')) {
                                    if (origPath.Contains(subTag)) {
                                        foundSubTag = true;
                                        newPath = $"{Core.Path}\\{tag}\\{subTag}\\{origPath.Split('\\').Last()}";
                                        filesToMove.Add([origPath, newPath, tag, subTag]);
                                        Console.WriteLine($"Prepared {origPath.Replace(Core.Path, "")}");
                                    }
                                }
                            }

                            if (foundSubTag)
                                continue;

                            newPath = $"{Core.Path}\\{tag}\\{origPath.Split('\\').Last()}";
                            filesToMove.Add([origPath, newPath, tag]);
                            Console.WriteLine($"Prepared: {origPath.Replace(Core.Path, "")}");
                        }
                    }
                }

                foreach (var fileToMove in filesToMove) {
                    int count = 0;
                    string origPath = fileToMove[0];
                    string newPath = fileToMove[1];
                    string tag = fileToMove[2];
                    string subTag = "";
                    if (fileToMove.Length > 3)
                        subTag = fileToMove[3];

                    while (File.Exists(newPath)) {
                        count++;
                        string fileName = origPath.Split('\\').Last();
                        fileName = fileName.Insert(fileName.Length - 4, count.ToString());
                        if (!string.IsNullOrEmpty(subTag))
                            newPath = $"{Core.Path}\\{tag}\\{subTag}\\{fileName}";
                        else
                            newPath = $"{Core.Path}\\{tag}\\{fileName}";
                        Console.WriteLine($"Path Already Used, Adjusted: {newPath.Replace(Core.Path, "")}");
                    }

                    count = 0;
                    if (!File.Exists(newPath)) {
                        if (!Directory.Exists($"{Core.Path}\\{tag}")) {
                            Console.WriteLine($"{tag} Not Found, Created: {newPath.Replace(Core.Path, "")}");
                        }

                        if (!Directory.Exists($"{Core.Path}\\{tag}\\{subTag}")) {
                            Console.WriteLine($"{subTag} Not Found, Created: {newPath.Replace(Core.Path, "")}");
                        }

                        File.Move(origPath, newPath);
                        Console.WriteLine($"Moved: {origPath.Replace(Core.Path, "")} => {newPath.Replace(Core.Path, "")}");
                    }
                    else {
                        Console.WriteLine($"Error - Ignoring: New Path Already Exists: {origPath.Replace(Core.Path, "")} => {newPath.Replace(Core.Path, "")}");
                    }
                }

                filesToMove.Clear();
                Console.WriteLine($"Moving Complete!");
            }
            else {
                Console.WriteLine($"Not a valid path: {Core.Path}");
            }
        }
    }
}
