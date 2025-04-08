using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityConsole.Ext
{
    public static class ResizeImage
    {
        public static string FileType = string.Empty;
        public static string ResizeAmount = string.Empty;
        public static string AppendName = string.Empty;

        public static void Process() {
            if (!Core.RunAgain) {
                Core.Path = string.Empty;
                FileType = string.Empty;

                Console.WriteLine("What is the starting directory?");
                Core.Path = Console.ReadLine();

                Console.WriteLine("What type of files are being changed?");
                FileType = "." + Console.ReadLine();

                Console.WriteLine("What is the new size?");
                ResizeAmount = Console.ReadLine();

                if (string.IsNullOrEmpty(ResizeAmount)) {
                    Console.WriteLine("A new size must be supplied. Aborting...");
                    return;
                }

                Console.WriteLine("What would you like to append to the file name?");
                AppendName = Console.ReadLine();

                Core.RunAgain = false;
            }

            if (Directory.Exists(Core.Path)) {
                foreach (var file in Directory.GetFiles(Core.Path, "*", SearchOption.AllDirectories)) {
                    FileInfo info = new FileInfo(file);
                    if (!info.Name.Contains(FileType)) {
                        Console.WriteLine($"Error - Ignoring: File Type Doesn't Match: {info.FullName}");
                        continue;
                    }

                    var image = Image.FromFile(info.FullName);
                    var bitmap = new Bitmap(image);
                    image.Dispose();

                    var newBitmap = new Bitmap(bitmap, new Size(int.Parse(ResizeAmount), int.Parse(ResizeAmount)));
                    bitmap.Dispose();

                    var newName = $"{info.FullName.Replace($"{info.Name}", $"{info.Name.Replace(".png", "")}{AppendName}{FileType}")}";

                    try {
                        if (File.Exists(newName)) {
                            File.Delete(newName);
                        }

                        newBitmap.Save(newName, ImageFormat.Png);
                        newBitmap.Dispose();

                        Console.WriteLine($"{info.FullName.Replace(Core.Path, "")} => {newName.Replace(Core.Path, "")}");
                    }
                    catch {
                        Console.WriteLine($"Error - Ignoring: New Path Already Exists: {info.FullName.Replace(Core.Path, "")} => {newName.Replace(Core.Path, "")}");
                    }
                }

                Console.WriteLine($"Resize Complete!");
            }
            else {
                Console.WriteLine($"Not a valid path: {Core.Path}");
            }
        }
    }
}
