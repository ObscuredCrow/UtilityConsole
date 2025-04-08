using UtilityConsole.Ext;

namespace UtilityConsole
{
    public static class Core
    {
        public static bool ShouldExit = false;
        public static bool RunAgain = false;

        public static string Path = string.Empty;
        public static TaskType Task = TaskType.None;

        private static string _task = string.Empty;

        public static void Initialize() {
            while (!ShouldExit) {

                if(Task == TaskType.None) {
                    Console.WriteLine("What type of task do you wish to perform? (Restructure, Move, Rename, Rotate, Resize, Convert)");
                    _task = Console.ReadLine();
                }

                if (!string.IsNullOrEmpty(_task)) {
                    try {
                        Task = (TaskType)Enum.Parse(typeof(TaskType), _task);

                        switch (Task) {
                            case TaskType.Restructure:
                                Restructure.Process();
                                break;

                            case TaskType.Move:
                                Move.Process();
                                break;

                            case TaskType.Rename:
                                Rename.Process();
                                break;

                            case TaskType.Rotate:
                                RotateImage.Process();
                                break;

                            case TaskType.Resize:
                                ResizeImage.Process();
                                break;

                            case TaskType.Convert:
                                ConvertImage.Process();
                                break;

                            case TaskType.None:
                                Console.WriteLine($"The provided task does not exist, aborting.");
                                break;
                        }
                    }
                    catch {
                        Console.WriteLine($"The provided task does not exist, aborting.");
                    }
                }
                else {
                    Console.WriteLine($"A task must be provided, options are: Restructure, Move, Rename, Rotate, Resize, or Convert");
                }

                Console.WriteLine($"Hit R to Run Again - Hit N for New Task - Hit H for Help");

                var command = Console.ReadKey();
                Console.WriteLine("");
                Console.WriteLine("");

                if (command.Key == ConsoleKey.R) {
                    RunAgain = Task != TaskType.None;
                    ShouldExit = false;
                }
                else if (command.Key == ConsoleKey.N || command.Key == ConsoleKey.H) {
                    if (command.Key == ConsoleKey.H) {
                        Console.WriteLine($"Tasks: Restructure, Move, Rename");
                        Console.WriteLine($"All of the different task types that this utility provides.");
                        Console.WriteLine("");
                        Console.WriteLine($"Restructure: Start Path, Exclude Path, File Type, First Separator, Second Separator");
                        Console.WriteLine($"Renames all supplied file types in a path and subpaths, splits based on separators. The excluded path will not add to the file naming.");
                        Console.WriteLine("");
                        Console.WriteLine($"Move: Path, Tags, Subtags");
                        Console.WriteLine($"Moves files inside the path to any folder sharing the tag and/or subtags, subtags are within tags.");
                        Console.WriteLine("");
                        Console.WriteLine($"Rename: Path, Replace, With");
                        Console.WriteLine($"Renames all files within the given path by finding the Replace and switching it with With.");
                        Console.WriteLine("");
                        Console.WriteLine($"Rotate: Path, File Type, Degrees");
                        Console.WriteLine($"Rotates all images of supplied file type in path based on the supplied degrees.");
                        Console.WriteLine("");
                        Console.WriteLine($"Resize: Path, File Type, Size");
                        Console.WriteLine($"Resizes all images of supplied file type in path based to the supplied size.");
                        Console.WriteLine("");
                        Console.WriteLine($"Convert: Path, File Type, Convert Type");
                        Console.WriteLine($"Converts all images of supplied file type in path based to the supplied type.");
                        Console.WriteLine("");
                    }

                    Task = TaskType.None;
                    RunAgain = false;
                    ShouldExit = false;
                }
                else {
                    ShouldExit = true;
                }
            }
        }
    }

    public enum TaskType { 
        None,
        Restructure,
        Move,
        Rename,
        Rotate,
        Resize,
        Convert
    }
}
