using AutoMapper;
using Identity.Dto;
using Identity.Models;
using System.Text;
using XSystem.Security.Cryptography;

namespace Identity.Mapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Account, AccountDTO>().ReverseMap();
            CreateMap<RegisterAccoutDTO, Account>()
                .ForMember(dest => dest.PassWord, act => act.MapFrom(src => getHasedPassword(src.PassWord)));
            CreateMap<RegisterAccoutDTO, AccountDTO>().ReverseMap();
        }

        public byte[] getHasedPassword(string passWord)
        {
            var md5 = new MD5CryptoServiceProvider();
            var data = Encoding.ASCII.GetBytes(passWord);
            var hashedPassWord = md5.ComputeHash(data);

            return hashedPassWord;
        }
    }
}
