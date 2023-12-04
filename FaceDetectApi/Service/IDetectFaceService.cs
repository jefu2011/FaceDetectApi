namespace FaceDetectApi.Service
{
    public interface IDetectFaceService
    {
        Task<string> DectFace(IFormFile file);
    }
}
