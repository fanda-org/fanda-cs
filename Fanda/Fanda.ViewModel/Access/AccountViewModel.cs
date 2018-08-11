using System.Collections.Generic;

namespace Fanda.ViewModel.Access
{
    public class IdentityResult
    {
        public bool Succeeded { get; set; }

        public IEnumerable<IdentityError> Errors { get; set; }

        public static IdentityResult Success
        {
            get
            {
                return new IdentityResult { Succeeded = true, Errors = null };
            }
        }

        public static IdentityResult Failed(params IdentityError[] errors)
        {
            return new IdentityResult { Succeeded = false, Errors = errors };
        }

        //public string ToJson()
        //{
        //    return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        //}

        //public static IdentityResult FromJson(string data)
        //{
        //    return Newtonsoft.Json.JsonConvert.DeserializeObject<IdentityResult>(data);
        //}
    }

    public class IdentityError
    {
        public string Code { get; set; }
        public string Description { get; set; }

        //public string ToJson()
        //{
        //    return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        //}

        //public static IdentityError FromJson(string data)
        //{
        //    return Newtonsoft.Json.JsonConvert.DeserializeObject<IdentityError>(data);
        //}
    }

    public class SignInResult
    {
        public bool Succeeded { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsNotAllowed { get; set; }
        public bool RequiresTwoFactor { get; set; }

        //public string ToJson()
        //{
        //    return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        //}

        //public static SignInResult FromJson(string data)
        //{
        //    return Newtonsoft.Json.JsonConvert.DeserializeObject<SignInResult>(data);
        //}
    }
}