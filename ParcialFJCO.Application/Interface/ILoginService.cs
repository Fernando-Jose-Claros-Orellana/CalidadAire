using ParcialFJCO.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParcialFJCO.Application.Interface
{
    public interface ILoginService
    {
        public Task<ResponseT> Login(LoginUserN request);
    }
}
