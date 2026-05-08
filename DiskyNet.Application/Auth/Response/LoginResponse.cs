using System;
using System.Collections.Generic;
using System.Text;

namespace DiskyNet.Application.Auth.Reponse
{
    public record LoginResponse(string AccessToken, string FullName);
}
