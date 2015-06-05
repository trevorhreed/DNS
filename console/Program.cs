using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Script.Serialization;

namespace MyApp
{
    class Program
    {
        static void Main(string[] args)
        {
			Mapper.CreateMap<BUser, FUser>()
				.ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.handle))
				.ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.pass));
			Mapper.CreateMap<FUser, BUser>()
				.ForMember(dest => dest.handle, opt => opt.MapFrom(src => src.Username))
				.ForMember(dest => dest.pass, opt => opt.MapFrom(src => src.Password));

			FUser fUserConverted = Services.GetNewUser();
			Console.WriteLine(fUserConverted);

			BUser bUserConverted = fUserConverted;
			Console.WriteLine(bUserConverted);

			Console.WriteLine("\n\n\nPress any key to exit.");
			Console.ReadKey(true);
        }
    }

	class Services
	{
		public static BUser GetNewUser()
		{
			BUser bUser = new BUser();
			bUser.handle = "trevorhreed";
			bUser.pass = "123";
			bUser.email = "trevorhreed@gmail.com";
			return bUser;
		}
	}

	class BUser
	{
		public string handle { get; set; }
		public string pass { get; set; }
		public string email { get; set; }

		public override string ToString()
		{
			return "[handle: " + this.handle + ", pass: " + this.pass + ", email: " + this.email + "]";
		}
	}

	class FUser
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }

		public static implicit operator FUser(BUser bUser)
		{
			return Mapper.Map<FUser>(bUser);
		}

		public static implicit operator BUser(FUser fUser)
		{
			return Mapper.Map<BUser>(fUser);
		}

		public override string ToString()
		{
			return "[Username: " + this.Username + ", Password: " + this.Password + ", Email: " + this.Email + "]";
		}
	}
}
