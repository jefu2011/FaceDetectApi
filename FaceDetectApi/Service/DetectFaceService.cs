using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace FaceDetectApi.Service
{
    public class DetectFaceService : IDetectFaceService
    {
        private readonly CascadeClassifier haarCascade;

        public DetectFaceService()
        {
            haarCascade = new CascadeClassifier("haarcascade_frontalface_default.xml");
        }

        public async Task<string> DectFace(IFormFile file)
        {
            using(var strem = new MemoryStream())
            {
                await file.CopyToAsync(strem);
                var bytes = strem.ToArray();
                var localFileName = "inputFace.jpg";
                if (File.Exists(localFileName) is false)
                {
                    File.Create(localFileName).Dispose();
                }
                    await File.WriteAllBytesAsync(localFileName, bytes);
                    Image<Bgr, Byte> grayFrame = new Image<Bgr, byte>(localFileName);
                    Rectangle[] faces = haarCascade.DetectMultiScale(grayFrame, 1.2, 10);
                if (faces.Count() == 0)
                {
                    return string.Empty;
                }


                    foreach (var face in faces)
                    {
                        grayFrame.Draw(face, new Bgr(255, 255, 0), 1);
                    }
                    var result = grayFrame.ToJpegData();
                var resultFileName = "faceResult.jpg";

                if (File.Exists(resultFileName) is false)
                {
                    File.Create(resultFileName).Dispose();
                }

                await File.WriteAllBytesAsync(resultFileName, result);
                return resultFileName;
            }
        }

    }
}
