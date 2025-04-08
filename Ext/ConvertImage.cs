using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityConsole.Ext
{
    public static class ConvertImage
    {
        public static string FileType = string.Empty;
        public static string ConvertType = string.Empty;
        public static string AppendName = string.Empty;

        public static void Process() {
            if (!Core.RunAgain) {
                Core.Path = string.Empty;
                FileType = string.Empty;

                Console.WriteLine("What is the starting directory?");
                Core.Path = Console.ReadLine();

                Console.WriteLine("What type of files are being changed?");
                FileType = "." + Console.ReadLine();

                Console.WriteLine("What type of file are they being changed to?");
                ConvertType = "." + Console.ReadLine();

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

                    var newName = $"{info.FullName.Replace($"{info.Name}", $"{info.Name.Replace($"{FileType}", "")}{AppendName}{ConvertType}")}";

                    try {
                        if (File.Exists(newName)) {
                            File.Delete(newName);
                        }

                        var format = 
                            ConvertType == "tiff" ? ImageFormat.Tiff :
                            ConvertType == "jpeg" ? ImageFormat.Jpeg :
                            ConvertType == "bmp" ? ImageFormat.Bmp :
                            ConvertType == "gif" ? ImageFormat.Gif :
                            ConvertType == "ico" ? ImageFormat.Icon : ImageFormat.Png;

                        if (format == ImageFormat.Tiff || format == ImageFormat.Png) 
                            bitmap = ConvertAlpha(bitmap);
                        
                        bitmap.Save(newName, format);
                        bitmap.Dispose();

                        Console.WriteLine($"{info.FullName.Replace(Core.Path, "")} => {newName.Replace(Core.Path, "")}");
                    }
                    catch {
                        Console.WriteLine($"Error - Ignoring: New Path Already Exists: {info.FullName.Replace(Core.Path, "")} => {newName.Replace(Core.Path, "")}");
                    }
                }

                Console.WriteLine($"Convert Complete!");
            }
            else {
                Console.WriteLine($"Not a valid path: {Core.Path}");
            }
        }

        private static Bitmap ConvertAlpha(Bitmap bitmap) {
            var transparentBitmap = bitmap;
            for (int y = 0; y < bitmap.Height; y++) {
                for (int x = 0; x < bitmap.Width; x++) {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    if (pixelColor.R == 0 && pixelColor.G == 0 && pixelColor.B == 0) {
                        transparentBitmap.SetPixel(x, y, Color.FromArgb(0, pixelColor));
                    }
                    else {
                        transparentBitmap.SetPixel(x, y, pixelColor);
                    }
                }
            }
            return transparentBitmap;
        }
    }
}
