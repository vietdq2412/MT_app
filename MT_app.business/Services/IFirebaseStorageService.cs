using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using MT_app.core.ViewModel;

namespace MT_app.business.Services
{
    public interface IFirebaseStorageService
    {
        Task<string?> UploadFile(IFormFile formFile);
    }

    public class FirebaseStorageService : IFirebaseStorageService
    {
        private string ApiKey = "AIzaSyARJwlkSyenyzcuWH3ZVUdNkK3X1opRR4A";
        private string Bucket = "mt-app-6268.appspot.com";
        private string AuthEmail = "Mt-app@gmail.com";
        private string AuthPassword = "Vdq20!";

        public FirebaseStorageService()
        {
        }

        public async Task<string?> Upload(FileStream fileStream, string fileName)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(this.ApiKey));
            var authRs = await auth.SignInWithEmailAndPasswordAsync(this.AuthEmail, this.AuthPassword);

            var cancellation = new CancellationTokenSource();
            fileStream.Position = 0;
            var task = new FirebaseStorage(
                    this.Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(authRs.FirebaseToken),
                        ThrowOnCancel = true
                    }
                )
                .Child("images")
                .Child(fileName)
                .PutAsync(fileStream, cancellation.Token);

            task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Prgress: {e.Percentage} %");
            try
            {
                return await task;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<string?> UploadFile(IFormFile formFile)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (formFile.Length > 0)
            {
                string fileName = formFile.FileName;
                FileInfo fileInfo = new FileInfo(fileName);
                string fileNameWithPath = Path.Combine(path, fileName);

                await using (var fileStream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                    string? result = await Upload(fileStream, fileName);
                    fileStream.Close();
                    File.Delete(fileNameWithPath);

                    return result;
                }
            }

            return null;
        }
    }
}