using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TurkAzn.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TurkAzn.Data.Kimlik;
using TurkAzn.Web.Data;

namespace TurkAzn.Web.Hubs
{
    public class ChatHub : Hub
    {
        readonly ApplicationDbContext _dbContext;
        public ChatHub(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SendMessage(string alici,string yollayan, string message)
        {

            var aliciUser = await kullaniciGetir(alici);
            var yollayanUser = await kullaniciGetir(yollayan);
            Mesaj mesaj = new Mesaj()
            {
                  Alan_Kullanici = aliciUser,
                  YollanmaZamani = DateTime.Now,
                  TelefonNumarasiIceriyor = false,
                  MesajGovde = message,
                  Yollayan_Kullanici = yollayanUser,
                  OkunduMu = false,
                  
            };

            var deneme = message.Split(" ");
            foreach (string item in deneme)
            {
                string pattern = @"\(?\d{3}\)?[-\.]? *\d{3}[-\.]? *[-\.]?\d{4}";
                Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
                Match match = r.Match(item);
                if (match.Success)
                {
                    mesaj.TelefonNumarasiIceriyor = true;
                }
            }
            
            

            _dbContext.Mesajlar.Add(mesaj);
            await _dbContext.SaveChangesAsync();
            
            await Clients.User(yollayanUser.Id).SendAsync("ReceiveMessage", yollayan, message);
            await Clients.User(aliciUser.Id).SendAsync("ReceiveMessage", yollayan, message);
        }


        public async Task<Kullanici> kullaniciGetir(string kullaniciAdi) => await _dbContext.Kullanicilar.Where(x => x.UserName == kullaniciAdi).Include(x => x.Mesajlar).Include(x => x.Avatar).FirstOrDefaultAsync();

    }
}