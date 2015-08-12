using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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

		}
    }

	#region PaginationCollection stuff -- needs more work 
	public class PaginationCollection<B>
	{
		public List<B> Items;
		public int Total;
	}

	public class PaginatedCollection<F, B>
	{
		public List<F> Items;
		public string SearchTerm;
		public int RangeStart;
		public int RangeEnd;
		public int ItemCount;
		public int FirstIndex;
		public int PrevIndex;
		public List<int> LeadingPages = new List<int>();
		public int CurrentIndex;
		public List<int> TrailingPages = new List<int>();
		public int NextIndex;
		public int LastIndex;

		public PaginatedCollection(PaginationCollection<B> paginationCollection, string searchTerm, int currentIndex, int perPage)
		{
			this.Items = Mapper.Map<List<F>>(paginationCollection.Items);
			int pageCount = ((paginationCollection.Total + perPage - 1) / perPage);
			this.SearchTerm = searchTerm;
			this.CurrentIndex = currentIndex;
			this.RangeStart = currentIndex * perPage;
			this.RangeEnd = this.RangeStart + perPage;
			this.ItemCount = paginationCollection.Total;
			this.FirstIndex = currentIndex != 0 ? 0 : 0;
			this.PrevIndex = currentIndex > 1 ? currentIndex - 1 : -1;
			this.NextIndex = currentIndex < pageCount - 1 ? currentIndex + 1 : -1;
			this.LastIndex = currentIndex != pageCount - 1 ? pageCount - 1 : -1;
			for (int i = 0; i < currentIndex; i++)
			{
				LeadingPages.Add(i);
			}
			for (int i = currentIndex + 1; i < pageCount; i++)
			{
				TrailingPages.Add(i);
			}
		}
	}
	#endregion

	class EntityConversionExample
	{
		public static void run()
		{
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


		static FUser()
		{
			Mapper.CreateMap<BUser, FUser>()
				.ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.handle))
				.ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.pass));
			Mapper.CreateMap<FUser, BUser>()
				.ForMember(dest => dest.handle, opt => opt.MapFrom(src => src.Username))
				.ForMember(dest => dest.pass, opt => opt.MapFrom(src => src.Password));
		}

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
