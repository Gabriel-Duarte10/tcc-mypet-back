using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Models.FireBase;

namespace tcc_mypet_back.Services
{
    public class ImageService
    {
        private readonly IConfiguration _configuration;
        public ImageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async void DeleteImagesFireBase(List<string> imagesNames)
        {
            var secrets = _configuration.GetSection("FirebaseConfig").Get<FirebaseAuthenticationModel>();
                
            FirebaseAuthProvider firebaseConfiguration = new(new FirebaseConfig(secrets.ApiKey));

            FirebaseAuthLink authConfiguration = await firebaseConfiguration
                .SignInWithEmailAndPasswordAsync(secrets.Email, secrets.Password);

            CancellationTokenSource cancellationToken = new();

            foreach (var i in imagesNames)
            {
                await new FirebaseStorage(secrets.BucketName,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(authConfiguration.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child("images")
                .Child(i)
                .DeleteAsync();
            }
        }
        public async Task<List<ImageDto>> UploadImageFireBase(List<IFormFile> files)
        {
            try
            {
                var secrets = _configuration.GetSection("FirebaseConfig").Get<FirebaseAuthenticationModel>();

                
                FirebaseAuthProvider firebaseConfiguration = new(new FirebaseConfig(secrets.ApiKey));

                FirebaseAuthLink authConfiguration = await firebaseConfiguration
                    .SignInWithEmailAndPasswordAsync(secrets.Email, secrets.Password);

                CancellationTokenSource cancellationToken = new();

                List<ImageDto> listUrlImages = new List<ImageDto>();
                string imageFromFirebaseStorage = "";

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            var contentTypeParts = file.FileName.Split(".");
                            if (contentTypeParts.Length < 2)
                            {
                                throw new ArgumentException("O ContentType do arquivo é inválido ou não contém um subtipo.");
                            }

                            var fileName = Guid.NewGuid().ToString() + "." + contentTypeParts[1];


                            using (var stream = new MemoryStream(fileBytes))
                            {
                                FirebaseStorageTask storageManager = new FirebaseStorage(secrets.BucketName,
                                    new FirebaseStorageOptions
                                    {
                                        AuthTokenAsyncFactory = () => Task.FromResult(authConfiguration.FirebaseToken),
                                        ThrowOnCancel = true
                                    })
                                    .Child("images")
                                    .Child(fileName)
                                    .PutAsync(stream, cancellationToken.Token);

                                try
                                {
                                    imageFromFirebaseStorage = await storageManager;
                                }
                                catch (Exception ex) 
                                {  
                                    throw;
                                } 

                                listUrlImages.Add(new ImageDto(){
                                    UrlImage = imageFromFirebaseStorage,
                                    NameImage = fileName
                                });
                            }
                            
                        }
                    }
                }
                return listUrlImages;
            }
            catch (Exception ex) 
            {  
                throw;
            }          
        }
    }
}