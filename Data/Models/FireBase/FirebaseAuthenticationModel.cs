using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth;

namespace tcc_mypet_back.Data.Models.FireBase
{
    public sealed class FirebaseAuthenticationModel
    {
        public string ApiKey { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        public string BucketName { get; set; }

        public FirebaseAuthProvider CreateAuthProvider()
        {
            return new FirebaseAuthProvider(new FirebaseConfig(this.ApiKey));
        }

        public async Task<string> GetAuthenticationTokenAsync(FirebaseAuthProvider authProvider)
        {
            return (await authProvider.SignInWithEmailAndPasswordAsync(this.Email, this.Password)).FirebaseToken;
        }
    }
}